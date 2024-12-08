using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IOnAttackable, IAttackStateSettable, IPowerKindGettable, IAttackStateGettable
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

    void Start()
    {
        powerKind = CommonUtils.Instance.playerPower;
        currentPowerKind = powerKind.selectedPowerKinds[0];

        stateNames = new string[14] { "Base Layer.Idling", "Base Layer.Walking", "Base Layer.Sprinting",
        "Base Layer.Jumping", "Base Layer.Twisting", "Base Layer.Dashing",
        "Base Layer.NormalAttack1", "Base Layer.NormalAttack2", "Base Layer.NormalAttack3",
        "Base Layer.RisingWind", "Base Layer.RisingWater", "Base Layer.RisingFire",
        "Base Layer.SuperAttack1", "Base Layer.SuperAttack2" };
        stateHashes = new int[14];

        playerRenderer = body.GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < stateHashes.Length; i++)
        {
            stateHashes[i] = Animator.StringToHash(stateNames[i]);
        }
        stateNames = null;

        if (CommonUtils.Instance.onlyOneMode)
        {
            health = playerOnOnlyModeProperties.properties._health;
            playerCount = 1;
        }
        else
        {
            health = playerOnSwitchModeProperties.properties._health;
            playerCount = 2;
        }
        playerData = new SwitchData(animator, stateHashes, health);
        container = playerData.GetYourAnimationContainer(currentPowerKind);
        playerRenderer.material = RefToAssets.refs._skinsDictionary[(currentPowerKind, playerChar)];
        inputProcessor = new PlayInput(container, stateHashes);
        PlayerAttackBehaviours[] animatorStateMachineClones = animator.GetBehaviours<PlayerAttackBehaviours>();
        foreach (PlayerAttackBehaviours clone in animatorStateMachineClones)
        {
            clone.playerWeapon = weapon;
            clone.stateHashes = stateHashes;
        }
    }
    bool isOnGround;
    void Update()
    {
        if (GameManager.instance.gameOver || GameManager.instance.gamePause)
            return;
        inputProcessor.SetAxisInputValue
            (Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        inputProcessor.SetDirection(gameObject, head);
        inputProcessor.ToWalk();
        inputProcessor.ToJump(Input.GetKeyDown(KeyCode.Space), isOnGround);
        inputProcessor.ToDash(Input.GetMouseButtonDown(1));
        inputProcessor.ToSprint(Input.GetKey(KeyCode.LeftShift));
        inputProcessor.ToTurnOnUniqueSkill(Input.GetKeyDown(KeyCode.Q));
        inputProcessor.ToAnimateComboAttack(Input.GetMouseButtonDown(0), gameObject);
        inputProcessor.ToDoubleSuperAttack(Input.GetKeyDown(KeyCode.E));
        inputProcessor.ToChangeThePower(Input.GetKeyDown(KeyCode.F) && !CommonUtils.Instance.onlyOneMode,
                                        ref currentPowerKind, powerKind.selectedPowerKinds, playerChar,
                                        ref health, playerData, playerRenderer);
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

    float defaltDamagePercentageOfEnemy = 1.5f;
    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        if (mark != null)
        {
            CommonUtils.Instance.ToDealDamage(mark.Value, enemyCurrentPower, CharacterKind.Monster, ref health, defaltDamagePercentageOfEnemy);
            playerData.SetHealth(currentPowerKind, health);
        }
        else
        {
            mark = enemyCurrentPower;
            StartCoroutine(ResetTheMark());
        }
        ToReact();
    }
    private IEnumerator ResetTheMark()
    {
        yield return new WaitForSeconds(5);
        mark = null;
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
            CommonUtils.Instance.onlyOneMode = true;
        }
        else
        {
            GameManager.instance.gameOver = true;
            return;
        }
        GameManager.instance.gamePause = true;
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
                                    ref health, playerData, playerRenderer);
            GameManager.instance.gamePause = false;
        }
    }
}
public enum AttackState
{
    NormalAttack,
    SuperAttack,
    UniqueSkill
}