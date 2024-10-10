/*
//Não jogar fora
//09/10/2024
//Michael Moraes Sabino
using UnityEngine;

namespace TrilloBit3sIndieGames
{
    public class OpenSpikeTrap : MonoBehaviour
    {
        //Controla a velocidade de abertura em um momento específico (ex. o objeto abre rápido no início e desacelera no final) "Curva"
        public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) }); 
        public enum OpenDirection { x, y, z }
        public OpenDirection direction = OpenDirection.y;
        public float openDistance = 3f; //Até onde o objeto deve deslizar (mudar de direção inserindo um valor positivo ou negativo)
        public float openSpeedMultiplier = 2.0f; //Aumentar este valor fará com que o objeto abra mais rápido
        public Transform spikesBody; 

        bool open = false;

        Vector3 defaultSpikesPosition;
        Vector3 currentSpikesPosition;
        float openTime = 0;

        void Start()
        {
            if (spikesBody)
            {
                defaultSpikesPosition = spikesBody.localPosition;
            }

            GetComponent<Collider>().isTrigger = true;
        }

        void Update()
        {
            if (!spikesBody)
                return;

            if (openTime < 1)
            {
                openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
            }

            if (direction == OpenDirection.x)
            {
                spikesBody.localPosition = new Vector3(Mathf.Lerp(currentSpikesPosition.x, defaultSpikesPosition.x + (open ? openDistance : 0), openTime), spikesBody.localPosition.y, spikesBody.localPosition.z);
            }
            else if (direction == OpenDirection.y)
            {
                spikesBody.localPosition = new Vector3(spikesBody.localPosition.x, Mathf.Lerp(currentSpikesPosition.y, defaultSpikesPosition.y + (open ? openDistance : 0), openTime), spikesBody.localPosition.z);
            }
            else if (direction == OpenDirection.z)
            {
                spikesBody.localPosition = new Vector3(spikesBody.localPosition.x, spikesBody.localPosition.y, Mathf.Lerp(currentSpikesPosition.z, defaultSpikesPosition.z + (open ? openDistance : 0), openTime));
            }
        }

        //Ativa a função Main quando o Player entra na área de trigger
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                open = true;
                currentSpikesPosition = spikesBody.localPosition;
                openTime = 0;
            }
        }

        // Desativa a função Main quando o Player sai da área de trigger
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                open = false;
                currentSpikesPosition = spikesBody.localPosition;
                openTime = 0;
            }
        }
    }
}
*/