using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IOnAttackable, IAttackStateSettable, IPowerKindGettable, IAttackStateGettable, IHitCountForUsingSkillSettable
{
    SelectedPowerKind powerKind;
    PowerKind currentPowerKind;
    PowerKind? mark;
    CharacterKind playerChar = CharacterKind.Player;
    int playerCount;
    AnimationContainer container;
    PlayInput inputProcessor;
    SwitchData playerData;
    Animator animator;
    Renderer playerRenderer;
    [SerializeField]
    GameObject weapon, body;
    [SerializeField]
    Transform head;
    string[] stateNames;
    int[] stateHashes;

    float health;
    [SerializeField]
    CharacterProperties playerOnSwitchModeProperties, playerOnOnlyModeProperties;
    [SerializeField]
    DefaultValue defaltDamagePercentageOfEnemy;
    [SerializeField]
    SkillsUsingCondition superSkill, uniqueSkill;
    [SerializeField]
    DefaultValue numberOfUnitInCooldown, resetMarkTime_countBySecond;
    bool isInCooldown;
    int hitCount;
    bool onlyMode;

    void Start()
    {
        powerKind = CommonUtils.Instance.playerPower;
        currentPowerKind = powerKind.selectedPowerKinds[0];

        stateNames = new string[13] { "Base Layer.Idling", "Base Layer.Walking", "Base Layer.Sprinting",
        "Base Layer.Jumping", "Base Layer.Twisting", "Base Layer.Dashing",
        "Base Layer.NormalAttack1", "Base Layer.NormalAttack2", "Base Layer.NormalAttack3",
        "Base Layer.RisingWind", "Base Layer.RisingWater", "Base Layer.RisingFire",
        "Base Layer.SuperAttack" };
        stateHashes = new int[13];

        playerRenderer = body.GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < stateHashes.Length; i++)
        {
            stateHashes[i] = Animator.StringToHash(stateNames[i]);
        }
        stateNames = null;

        onlyMode = CommonUtils.Instance.onlyOneMode;

        if (onlyMode)
        {
            health = playerOnOnlyModeProperties.properties._health;
            playerCount = 1;
        }
        else
        {
            health = playerOnSwitchModeProperties.properties._health;
            playerCount = 2;
        }
        playerData = new SwitchData(animator, stateHashes, health, hitCount);
        container = playerData.GetYourAnimationContainer(currentPowerKind);
        playerRenderer.sharedMaterial = RefToAssets.refs._skinsDictionary[(currentPowerKind, playerChar)];
        inputProcessor = new PlayInput(container, stateHashes);
        PlayerAttackBehaviours[] animatorStateMachineClones = animator.GetBehaviours<PlayerAttackBehaviours>();
        foreach (PlayerAttackBehaviours clone in animatorStateMachineClones)
        {
            clone.playerWeapon = weapon;
            clone.stateHashes = stateHashes;
        }
        resetMarkTime = (int)resetMarkTime_countBySecond.value;
        numberOfUnit = (int)numberOfUnitInCooldown.value;
        StartCoroutine(AfterCooldown());
    }
    bool isOnGround;
    bool isAutoSwitchingOnDying;
    void Update()
    {
        if (GameManager.instance.gameOver || GameManager.instance.gamePause || isAutoSwitchingOnDying
            || (Cursor.lockState != CursorLockMode.Locked && Cursor.visible))
            return;
        inputProcessor.SetAxisInputValue
            (Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        inputProcessor.SetDirection(gameObject, head);
        inputProcessor.ToWalk();
        inputProcessor.ToJump(Input.GetKeyDown(KeyCode.Space), isOnGround);
        inputProcessor.ToDash(Input.GetMouseButtonDown(1));
        inputProcessor.ToSprint(Input.GetKey(KeyCode.LeftShift));
        inputProcessor.ToTurnOnUniqueSkill(currentPowerKind, playerData,
                                           Input.GetKeyDown(KeyCode.Q) && hitCount == uniqueSkill.afterHitCount,
                                           ref hitCount);
        inputProcessor.ToAnimateComboAttack(Input.GetMouseButtonDown(0), gameObject);
        inputProcessor.ToTurnOnSuperAttack(Input.GetKeyDown(KeyCode.E), ref isInCooldown);
        inputProcessor.ToChangeThePower(Input.GetKeyDown(KeyCode.F) && !onlyMode,
                                        ref currentPowerKind, powerKind.selectedPowerKinds, playerChar,
                                        ref health, ref hitCount, playerData, playerRenderer);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isOnGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isOnGround = false;
    }

    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        if (isAutoSwitchingOnDying)
            return;
        if (mark != null)
        {
            CommonUtils.Instance.ToDealDamage(mark.Value, enemyCurrentPower, CharacterKind.Monster, ref health, defaltDamagePercentageOfEnemy.value);
            playerData.SetHealth(currentPowerKind, health);
        }
        else
        {
            mark = enemyCurrentPower;
            GameManager.instance.Notify(TypeOfEvent.PlayerMarkChange, mark.ToString());
            StartCoroutine(ResetTheMark());
        }
        ToReact();
    }

    int resetMarkTime;
    private IEnumerator ResetTheMark()
    {
        yield return new WaitForSeconds(resetMarkTime);
        mark = null;
        GameManager.instance.Notify(TypeOfEvent.PlayerMarkChange, mark.ToString());
    }

    int reactTransitionHash = Animator.StringToHash("react");
    int reactStateHash = Animator.StringToHash("Base Layer.Reacting");
    void ToReact()
    {
        if (health <= 1)
        {
            ToDie();
            return;
        }
        container.TurnOnTemporaryAnimation(reactTransitionHash, reactStateHash);
        Debug.Log(health);
    }

    int dieTransitionHash = Animator.StringToHash("die");
    int dieStateHash = Animator.StringToHash("Base Layer.Dying");
    void ToDie()
    {
        playerCount--;
        container.TurnOnTemporaryAnimation(dieTransitionHash, dieStateHash);
        if (playerCount == 1)
        {
            onlyMode = true;
        }
        else
        {
            GameManager.instance.SetGameOverState(GameOverState.Lose);
            return;
        }
        isAutoSwitchingOnDying = true;
    }


    AttackState? playerCurrentAttack;

    public void SetAttackState(AttackState? newAttackState)
    {
        playerCurrentAttack = newAttackState;
    }

    public PowerKind GetPowerKind()
    {
        return currentPowerKind;
    }

    public AttackState? GetAttackState()
    {
        return playerCurrentAttack;
    }

    public void AutoChangePlayerCharacterAsDie()
    {
        if (playerCount > 0)
        {
            inputProcessor.ToChangeThePower(true,
                                    ref currentPowerKind, powerKind.selectedPowerKinds, playerChar,
                                    ref health, ref hitCount, playerData, playerRenderer);
            isAutoSwitchingOnDying = false;
        }
    }

    int numberOfUnit;
    float elapsedTime;
    IEnumerator AfterCooldown()
    {
        while (!GameManager.instance.gameOver)
        {
            yield return new WaitUntil(() => isInCooldown == true);
            for (int i = 0; i < numberOfUnit; i++)
            {
                GameManager.instance.Notify(TypeOfEvent.PlayerSuperSkillStatusChange, elapsedTime);
                yield return new WaitForSeconds(superSkill.cooldown_second / numberOfUnit);
                elapsedTime += superSkill.cooldown_second / numberOfUnit;
            }
            GameManager.instance.Notify(TypeOfEvent.PlayerSuperSkillStatusChange, elapsedTime);
            elapsedTime = 0;
            isInCooldown = false;
        }
    }

    public void SetHitCount()
    {
        hitCount++;
        hitCount = Mathf.Clamp(hitCount, 0, uniqueSkill.afterHitCount);
        playerData.SetHitCount(currentPowerKind, hitCount);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
public enum AttackState
{
    NormalAttack,
    SuperAttack,
    UniqueSkill
}