using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IOnAttackable, IAttackStateSettable, IPowerKindGettable
{
    SelectedPowerKind powerKind;
    PowerKind currentPowerKind;
    PowerKind? mark;
    CharacterKind playerChar = CharacterKind.Player;
    int playerCount;
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
        #region Temp
        powerKind = new SelectedPowerKind();
        powerKind.selectedPowerKinds[0] = PowerKind.Wind;
        powerKind.selectedPowerKinds[1] = PowerKind.Water;
        #endregion
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
        AnimationContainer container = playerData.GetYourAnimationContainer(currentPowerKind);
        playerRenderer.material = RefToAssets.refs._skinsDictionary[(currentPowerKind, playerChar)];
        inputProcessor = new PlayInput(container, stateHashes);
        AnimatorStateMachine[] animatorStateMachineClones = animator.GetBehaviours<AnimatorStateMachine>();
        foreach (AnimatorStateMachine clone in animatorStateMachineClones)
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

    public void OnBeAttacked(PowerKind enemyCurrentPower)
    {
        if (mark == null)
        {
            mark = enemyCurrentPower;
            StartCoroutine(ResetTheMark());
        }
        if (mark != enemyCurrentPower && mark != null)
        {
            CommonUtils.Instance.ToDealDamage(mark.Value, enemyCurrentPower, CharacterKind.Monster, ref health, 0);
            playerData.SetHealth(currentPowerKind, health);
        }
        ToReact();
    }
    private IEnumerator ResetTheMark()
    {
        yield return new WaitForSeconds(3);
        mark = null;
    }

    void ToReact()
    {
        if (health <= 0)
        {
            ToDie();
            return;
        }
        //animate react
    }

    void ToDie()
    {
        playerCount--;
        if (playerCount == 1)
            CommonUtils.Instance.onlyOneMode = true;
        //else: losing
        //animate die
        inputProcessor.ToChangeThePower(true,
                                        ref currentPowerKind, powerKind.selectedPowerKinds, playerChar,
                                        ref health, playerData, playerRenderer);
    }


    public AttackState? playerCurrentAttack { get; private set; }

    public void SetAttackState(AttackState? newAttackState)
    {
        playerCurrentAttack = newAttackState;
    }

    public PowerKind GetPowerKind()
    {
        return currentPowerKind;
    }
}
public enum AttackState
{
    NormalAttack,
    SuperAttack,
    UniqueSkill
}