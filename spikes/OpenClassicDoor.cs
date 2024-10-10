using UnityEngine;

public class OpenClassicDoor : MonoBehaviour
{
    //Abre suavemente uma porta
    //Controla a velocidade de abertura em um momento específico (ex. a porta abre rápido no início e desacelera no final)
    public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { 
                    new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) }); 
    public float openSpeedMultiplier = 2.0f; //Aumentar este valor fará com que a porta abra mais rápido
    public float doorOpenAngle = 90.0f; //Velocidade global de abertura da porta que multiplicará o openSpeedCurve

    bool open = false;
    bool enter = false;

    float defaultRotationAngle;
    float currentRotationAngle;
    float openTime = 0;

    void Start()
    {
        defaultRotationAngle = transform.localEulerAngles.y;
        currentRotationAngle = transform.localEulerAngles.y;

        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (openTime < 1)
        {
            openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(currentRotationAngle, defaultRotationAngle + (open ? doorOpenAngle : 0), openTime), transform.localEulerAngles.z);

        if (Input.GetKeyDown(KeyCode.F) && enter)
        {
            open = !open;
            currentRotationAngle = transform.localEulerAngles.y;
            openTime = 0;
        }
    }

    // Exibe uma mensagem informativa simples quando o jogador está dentro da área de trigger (Isso é apenas para fins de teste, para que você possa removê-lo)
    void OnGUI()
    {
        if (enter)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 30), "Press 'F' to " + (open ? "close" : "open") + " the door");
        }
    }

    //Ativa a função Main quando o Player entra na área de trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    // Desativa a função Main quando o Player sai da área de trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}