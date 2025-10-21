using System.Collections;
using UnityEngine;

public class ScarySceneCollider : MonoBehaviour
{
    public string playerTag = "Player";
    public SecondSequenceController secondSequenceController;
    public EndScreen endScreen;

    [Header("Door")]
    public DoorHandleController doorHandleScaryScene;   // sua porta
    public AudioSource doorScarySound;                  // som de porta/ambiente (opcional)
    public float doorOpenSpeed = 0.4f;                  // bem devagar
    public float doorOpenEpsilon = 1.0f;                // tolerância (graus)

    [Header("Corridor Light (flicker)")]
    public Light corridorLight;                         // Light (Type = Spot)
    public float flickerDuration = 3.0f;                // dura quantos segundos
    public Vector2 flickerIntervalRange = new Vector2(0.04f, 0.12f);
    public Vector2 intensityRange = new Vector2(0.2f, 1.4f);

    [Header("Enemy")]
    public GameObject enemyObject;                      // inimigo
    public Transform enemyAppearPoint;                  // onde ele “aparece” no corredor
    public Transform enemyJumpPoint;                    // onde ele teleporta (perto do player/câmera)
    public float appearDelayAfterDoor = 0.4f;           // pausa breve após abrir a porta
    public float teleportDelay = 0.35f;                 // tempo entre aparecer e teleportar
    public AudioSource enemyJumpScareSource;            // som do jumpscare
    public Transform player;                            // player/câmera
    public PlayerMovemetController pmc;

    [Header("Facing")]
    public float modelYawOffset = -90f;                 // ajuste se o modelo estiver 90° pro lado
    public float faceSpeed = 10f;                       // velocidade para encarar o player

    [Header("Extra SFX")]
    public AudioSource scarySound;                      // som ambiente da cena master (opcional)

    bool hasTriggered = false;
    Coroutine flickerRoutine;

    [Header("Wait Conditions Before Teleport")]
    public bool requireProximity = true;
    public float triggerDistance = 3.0f;       // distance to enemyAppearPoint
    public bool requireLook = true;
    public float lookMaxAngle = 20f;           // quão precisamente o player deve olhar pro inimigo
    public float minStareTime = 0.2f;          // por quanto tempo as condições devem se manter
    public float failSafeTimeout = 6f;         // teleporta de qualquer jeito após esse tempo

    [Header("Behavior While Waiting")]
    public bool facePlayerContinuously = true; // inimigo fica encarando o player enquanto “stuck”
    public AudioSource laughTV; 
    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        // Só dispara se o inimigo já foi revelado na fase 2
        if (secondSequenceController == null || !secondSequenceController.EnemyRevealed)
            return;

        hasTriggered = true;
        StartCoroutine(ScaryCutSceneEndGame());
    }

    IEnumerator ScaryCutSceneEndGame()
    {
        // 1) Abrir porta devagar
        if (doorHandleScaryScene)
        {
            doorHandleScaryScene.speed = doorOpenSpeed;
            doorHandleScaryScene.isOpen = true;
            if (doorScarySound && doorScarySound.clip)
                doorScarySound.PlayOneShot(doorScarySound.clip);

            while (Mathf.Abs(Mathf.DeltaAngle(
                doorHandleScaryScene.transform.localEulerAngles.y,
                doorHandleScaryScene.openY)) > doorOpenEpsilon)
            {
                yield return null;
            }
        }

        // 2) Flicker da luz (paralelo)
        if (corridorLight)
            flickerRoutine = StartCoroutine(FlickerLight(corridorLight, flickerDuration, flickerIntervalRange, intensityRange));

        if (scarySound && scarySound.clip)
            scarySound.PlayOneShot(scarySound.clip);

        // 3) Inimigo aparece e fica “preso” no ponto
        yield return new WaitForSeconds(appearDelayAfterDoor);

        Coroutine faceLoop = null;

        if (enemyObject)
        {
            if (enemyAppearPoint)
                enemyObject.transform.SetPositionAndRotation(enemyAppearPoint.position, enemyAppearPoint.rotation);

            enemyObject.SetActive(true);
            pmc.canMove = false;
            // garante animação atualizando mesmo fora de câmera
            var anim = enemyObject.GetComponent<Animator>();
            if (anim) anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            // muda layer para Default (e filhos)
            enemyObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in enemyObject.GetComponentsInChildren<Transform>(true))
                child.gameObject.layer = LayerMask.NameToLayer("Default");

            // trava movimento físico/navmesh se existir
            var agent = enemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent) agent.isStopped = true;
            var rb = enemyObject.GetComponent<Rigidbody>();
            if (rb) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; rb.isKinematic = true; }

            // encara player imediatamente (com offset de modelo)
            if (player)
                LookAtFlatOffset(enemyObject.transform, player.position, modelYawOffset);

            // mantém encarando enquanto espera (opcional)
            if (facePlayerContinuously && player)
                faceLoop = StartCoroutine(FacePlayerContinuously(enemyObject.transform, player, modelYawOffset));

            // ---- Espera condições para teleporte (ou timeout) ----
            float start = Time.time;
            float satisfiedSince = -1f;

            while (Time.time - start < failSafeTimeout)
            {
                bool okDist = !requireProximity || DistanceToAppearPoint(enemyObject.transform.position) <= triggerDistance;
                bool okLook = !requireLook || IsPlayerLookingAt(enemyObject.transform.position);

                if (okDist && okLook)
                {
                    if (satisfiedSince < 0f) satisfiedSince = Time.time;
                    if (Time.time - satisfiedSince >= minStareTime) break;
                }
                else
                {
                    satisfiedSince = -1f;
                }

                yield return null;
            }
        }

        // 4) Teleporte + jumpscare (agora, após as condições)
        if (enemyObject && enemyJumpPoint)
        {
            enemyObject.transform.SetPositionAndRotation(enemyJumpPoint.position, enemyJumpPoint.rotation);
            if (player)
                LookAtFlatOffset(enemyObject.transform, player.position, modelYawOffset);
        }

        if (faceLoop != null) { StopCoroutine(faceLoop); faceLoop = null; }

        if (enemyJumpScareSource && enemyJumpScareSource.clip)
            enemyJumpScareSource.PlayOneShot(enemyJumpScareSource.clip);

        // 5) Encerrar flicker e finalizar
        if (flickerRoutine != null) { StopCoroutine(flickerRoutine); flickerRoutine = null; }
        if (corridorLight) { corridorLight.enabled = true; corridorLight.intensity = intensityRange.y; }

        if (enemyJumpScareSource && enemyJumpScareSource.clip)
            endScreen.PlayEnd();
            yield return new WaitForSeconds(enemyJumpScareSource.clip.length * 0.9f);

        laughTV.Play();
        Debug.Log(">> FIM DE JOGO POR CENA MASTER <<");
    }

    // ===== Helpers =====
    void LookAtFlatOffset(Transform t, Vector3 targetPos, float yawOffsetDeg)
    {
        Vector3 flat = new Vector3(targetPos.x, t.position.y, targetPos.z);
        Vector3 dir = (flat - t.position).normalized;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion look = Quaternion.LookRotation(dir);
        Quaternion offset = Quaternion.Euler(0f, yawOffsetDeg, 0f);
        t.rotation = look * offset;
    }

    // wrapper sem offset (caso você chame LookAtFlat em outros lugares)
    void LookAtFlat(Transform t, Vector3 targetPos)
    {
        LookAtFlatOffset(t, targetPos, 0f);
    }

    float DistanceToAppearPoint(Vector3 enemyPos)
    {
        if (!player) return Mathf.Infinity;
        Vector3 p = player.position;
        Vector3 refPos = enemyAppearPoint ? enemyAppearPoint.position : enemyPos;
        return Vector3.Distance(p, refPos);
    }

    bool IsPlayerLookingAt(Vector3 targetPos)
    {
        if (!player || !Camera.main) return false;
        Vector3 to = (targetPos - Camera.main.transform.position).normalized;
        float angle = Vector3.Angle(Camera.main.transform.forward, to);
        return angle <= lookMaxAngle;
    }

    IEnumerator FacePlayerContinuously(Transform enemy, Transform pl, float yawOffsetDeg)
    {
        while (enemy && pl)
        {
            Vector3 flat = new Vector3(pl.position.x, enemy.position.y, pl.position.z);
            Vector3 dir = (flat - enemy.position).normalized;

            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, yawOffsetDeg, 0f);
                enemy.rotation = Quaternion.Slerp(enemy.rotation, look, Time.deltaTime * faceSpeed);
            }
            yield return null;
        }
    }

    IEnumerator FlickerLight(Light l, float duration, Vector2 intervalRange, Vector2 intensityRange)
    {
        float t = 0f;
        bool state = true;
        float baseIntensity = l.intensity;

        while (t < duration)
        {
            state = !state;
            l.enabled = state;
            if (state)
                l.intensity = Random.Range(intensityRange.x, intensityRange.y);

            float step = Random.Range(intervalRange.x, intervalRange.y);
            t += step;
            yield return new WaitForSeconds(step);
        }

        l.enabled = true;
        l.intensity = baseIntensity;
    }
}
