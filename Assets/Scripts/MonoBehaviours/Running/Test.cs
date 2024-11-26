using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    NaturePowerKind powerKind;
    PlayInput inputMoving;
    PlayerData playerData;
    Animator animator;
    Renderer playerRenderer;
    [SerializeField]
    GameObject weapon, body;
    [SerializeField]
    Transform head;
    string[] stateNames;
    int[] stateHashes;
    void Start()
    {
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
        playerData = new PlayerData(animator, stateHashes, powerKind.unselectedKind);
        AnimationContainer container = playerData.GetYourAnimationContainer(powerKind.powerKind);
        playerRenderer.material = RefToAssets.refs._skinsDictionary[(powerKind.powerKind, CharacterKind.Player)];
        inputMoving = new PlayInput(container, stateHashes);
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
        inputMoving.SetAxisInputValue
            (Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        inputMoving.SetDirection(gameObject, head);
        inputMoving.ToWalk();
        inputMoving.ToJump(Input.GetKeyDown(KeyCode.Space), isOnGround);
        inputMoving.ToDash(Input.GetMouseButtonDown(1));
        inputMoving.ToSprint(Input.GetKey(KeyCode.LeftShift));
        inputMoving.ToTurnOnUniqueSkill(Input.GetKeyDown(KeyCode.Q));
        inputMoving.ToAnimateComboAttack(Input.GetMouseButtonDown(0), gameObject);
        inputMoving.ToDoubleSuperAttack(Input.GetKeyDown(KeyCode.E));
        inputMoving.ToChangeThePower(Input.GetKeyDown(KeyCode.F) && !CommonMethods.Instance.onlyOneMode, ref powerKind.powerKind, powerKind.unselectedKind, playerData, playerRenderer);
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

    private void OnDestroy()
    {
        stateNames = null;
    }
}
