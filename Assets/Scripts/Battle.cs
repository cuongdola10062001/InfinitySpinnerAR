 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.Pool;

public class Battle : MonoBehaviourPun
{
    public Spinner spinner;
    public GameObject ui_3D_GameObject;
    public GameObject dealthPanelUIPrefab;
    private GameObject dealthPanelUIGameObject;

    private Rigidbody _rb;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Txt;

    public float common_Damage_Coefficient=0.04f;

    public bool isAttacker;
    public bool isDefender;
    public bool isDead = false;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient_Attack = 10f;
    public float getDamage_Coefficient_Attack = 1.2f;

    public float doDamage_Coefficient_Defender = 0.75f;
    public float getDamage_Coefficient_Defender = 0.2f;

    private void Awake()
    {
        startSpinSpeed = spinner.spinSpeed;
        currentSpinSpeed = spinner.spinSpeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if(gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        } else if(gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;
            spinner.spinSpeed = 4400;

            startSpinSpeed = spinner.spinSpeed;
            currentSpinSpeed = spinner.spinSpeed;

            spinSpeedRatio_Txt.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                Vector3 effectPosition = (gameObject.transform.position + collision.transform.position) / 2 + new Vector3(0, 0.05f, 0);

                GameObject collisionEffectGameObject = GetPooledObject();

                if(collisionEffectGameObject != null)
                {
                    collisionEffectGameObject.transform.position = effectPosition;
                    collisionEffectGameObject.SetActive(true);
                    collisionEffectGameObject.GetComponentInChildren<ParticleSystem>().Play();

                    StartCoroutine(DeactiveAfterSeconds(collisionEffectGameObject, 0.5f));
                }
            }


            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if(mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You damage the other player");
                float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_Damage_Coefficient;

                if (isAttacker)
                {
                    default_Damage_Amount *= doDamage_Coefficient_Attack;
                }
                else if (isDefender)
                {
                    default_Damage_Amount *= doDamage_Coefficient_Defender;
                }

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {

                    // Apply Damage to the slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_Damage_Amount);
                }
            } else
            {
                Debug.Log("You get damage");
            }
        }
    }

    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                damageAmount *= getDamage_Coefficient_Attack;

                if (damageAmount > 1000)
                {
                    damageAmount = 400f;
                }
            }
            else if (isDefender)
            {
                damageAmount *= getDamage_Coefficient_Defender;
            }

            spinner.spinSpeed -= damageAmount;
            currentSpinSpeed = spinner.spinSpeed;
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinSpeedRatio_Txt.text = currentSpinSpeed.ToString("F0") + "/"+ startSpinSpeed;

            if (currentSpinSpeed < 100)
            {
                // Die
                Die();
            }
        }
    }
    private void Die()
    {
        isDead = true;

        GetComponent<MovementController>().enabled = false;
        _rb.freezeRotation = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        spinner.spinSpeed = 0f;

        ui_3D_GameObject.SetActive(false);

        if (photonView.IsMine)
        {
            // countdown for respawn
            StartCoroutine(ReSpawn());
        }
    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        if(dealthPanelUIGameObject == null)
        {
            dealthPanelUIGameObject = Instantiate(dealthPanelUIPrefab, canvasGameObject.transform);

        } else
        {
            dealthPanelUIGameObject.SetActive(true);
        }

        Text respawnTimeText = dealthPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 8.0f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");
            GetComponent<MovementController>().enabled = false;
        }

        dealthPanelUIGameObject.SetActive(false);
        GetComponent<MovementController>().enabled = true;
        photonView.RPC("ReBorn", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void ReBorn()
    {
        spinner.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinner.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Txt.text = currentSpinSpeed + "/" + startSpinSpeed;

        _rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        ui_3D_GameObject.SetActive(true);

        isDead = false;
    }


    public List<GameObject> pooledObjects;
    public int amountToPool = 8;
    public GameObject CollisionEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
        _rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            pooledObjects = new List<GameObject>();

            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(CollisionEffectPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject()
    {
        for(int i=0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    IEnumerator DeactiveAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
    }
}
