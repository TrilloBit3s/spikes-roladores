//Michael Moraes Sabino - Trillobit3s@gmail.com - 09/10/2024 - Script para rolar objetos para qualquer direção

using UnityEngine;

namespace TrilloBit3sIndieGames
{
    public class SpikesArray : MonoBehaviour
    {
        public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { 
                        new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) });
        public enum OpenDirection { x, y, z }
        public OpenDirection direction = OpenDirection.y;
        public Collider colliderToDisable; // Variável para o Collider específico

        public float[] openDistances; 
        public float[] openSpeedMultipliers; 
        public Transform[] spikesBodies; 
        
        private bool open = false;
        private Vector3[] defaultSpikesPositions;
        private Vector3[] currentSpikesPositions;
        private float[] openTimes; 

        void Start()
        {
            if (spikesBodies.Length > 0)
            {
                defaultSpikesPositions = new Vector3[spikesBodies.Length];
                currentSpikesPositions = new Vector3[spikesBodies.Length];
                openTimes = new float[spikesBodies.Length];

                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        defaultSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }
            }

            GetComponent<Collider>().isTrigger = true; // Certifique-se de que este collider seja um trigger
        }

        void Update()
        {
            if (spikesBodies.Length == 0) return;

            for (int i = 0; i < spikesBodies.Length; i++)
            {
                if (!spikesBodies[i]) continue;

                if (openTimes[i] < 1)
                {
                    openTimes[i] += Time.deltaTime * (i < openSpeedMultipliers.Length ? openSpeedMultipliers[i] : 1) * openSpeedCurve.Evaluate(openTimes[i]);
                }

                float currentOpenDistance = (i < openDistances.Length) ? openDistances[i] : 0;

                if (direction == OpenDirection.x)
                {
                    spikesBodies[i].localPosition = new Vector3(Mathf.Lerp(currentSpikesPositions[i].x, defaultSpikesPositions[i].x + (open ? currentOpenDistance : 0), openTimes[i]), spikesBodies[i].localPosition.y, spikesBodies[i].localPosition.z);
                }
                else if (direction == OpenDirection.y)
                {
                    spikesBodies[i].localPosition = new Vector3(spikesBodies[i].localPosition.x, Mathf.Lerp(currentSpikesPositions[i].y, defaultSpikesPositions[i].y + (open ? currentOpenDistance : 0), openTimes[i]), spikesBodies[i].localPosition.z);
                }
                else if (direction == OpenDirection.z)
                {
                    spikesBodies[i].localPosition = new Vector3(spikesBodies[i].localPosition.x, spikesBodies[i].localPosition.y, Mathf.Lerp(currentSpikesPositions[i].z, defaultSpikesPositions[i].z + (open ? currentOpenDistance : 0), openTimes[i]));
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("Player entrou no trigger.");
                open = true;

                // Mantém o isTrigger ativo
                if (colliderToDisable != null)
                {
                    colliderToDisable.isTrigger = true; // Garante que o collider específico seja um trigger
                   // Debug.Log("isTrigger ativado no collider específico.");
                }

                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        currentSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }

                for (int i = 0; i < openTimes.Length; i++)
                {
                    openTimes[i] = 0;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("Player saiu do trigger.");
                open = false;

                // Desativa o isTrigger do collider específico
                if (colliderToDisable != null)
                {
                    colliderToDisable.isTrigger = false; // Desativa o isTrigger
                    //Debug.Log("isTrigger desativado no collider específico.");
                }

                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        currentSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }

                for (int i = 0; i < openTimes.Length; i++)
                {
                    openTimes[i] = 0;
                }
            }
        }
    }
}

/*
using UnityEngine;

namespace TrilloBit3sIndieGames
{
    public class SpikesArray : MonoBehaviour
    {
        public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) });
        public enum OpenDirection { x, y, z }
        public OpenDirection direction = OpenDirection.y;
        public float openDistance = 3f;
        public float openSpeedMultiplier = 2.0f;

        public Transform[] spikesBodies; // Array para múltiplos spikes

        private bool open = false;

        private Vector3[] defaultSpikesPositions;
        private Vector3[] currentSpikesPositions;
        private float openTime = 0;

        void Start()
        {
            if (spikesBodies.Length > 0)
            {
                defaultSpikesPositions = new Vector3[spikesBodies.Length];
                currentSpikesPositions = new Vector3[spikesBodies.Length];

                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        defaultSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }
            }

            GetComponent<Collider>().isTrigger = true;
        }

        void Update()
        {
            if (spikesBodies.Length == 0)
                return;

            if (openTime < 1)
            {
                openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
            }

            for (int i = 0; i < spikesBodies.Length; i++)
            {
                if (!spikesBodies[i])
                    continue;

                if (direction == OpenDirection.x)
                {
                    spikesBodies[i].localPosition = new Vector3(Mathf.Lerp(currentSpikesPositions[i].x, defaultSpikesPositions[i].x + (open ? openDistance : 0), openTime), spikesBodies[i].localPosition.y, spikesBodies[i].localPosition.z);
                }
                else if (direction == OpenDirection.y)
                {
                    spikesBodies[i].localPosition = new Vector3(spikesBodies[i].localPosition.x, Mathf.Lerp(currentSpikesPositions[i].y, defaultSpikesPositions[i].y + (open ? openDistance : 0), openTime), spikesBodies[i].localPosition.z);
                }
                else if (direction == OpenDirection.z)
                {
                    spikesBodies[i].localPosition = new Vector3(spikesBodies[i].localPosition.x, spikesBodies[i].localPosition.y, Mathf.Lerp(currentSpikesPositions[i].z, defaultSpikesPositions[i].z + (open ? openDistance : 0), openTime));
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                open = true;
                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        currentSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }
                openTime = 0;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                open = false;
                for (int i = 0; i < spikesBodies.Length; i++)
                {
                    if (spikesBodies[i])
                    {
                        currentSpikesPositions[i] = spikesBodies[i].localPosition;
                    }
                }
                openTime = 0;
            }
        }
    }
}
*/