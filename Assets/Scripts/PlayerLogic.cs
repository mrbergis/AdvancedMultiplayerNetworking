using Mirror;
using UnityEngine;

public class PlayerLogic : NetworkBehaviour
{
    CharacterController m_characterController;
    
        float _horizontalInput;
        float _verticalInput;
    
        const float MOVEMENT_SPEED = 5.0f;
    
        Vector3 _movementInput;
        Vector3 _movement;
        Vector3 _heightMovement;
    
        const float JUMP_HEIGHT = 0.25f;
        const float GRAVITY = 0.981f;
        bool _jump = false;
    
        [SerializeField]
        GameObject _cameraObject;
    
        Color _playerColor = Color.white;
        
        void Start()
        {
            m_characterController = GetComponent<CharacterController>();
    
            if(isLocalPlayer && _cameraObject)
            {
                Camera.main.enabled = false;
                _cameraObject.SetActive(true);
            }
    
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if(meshRenderer)
            {
                _playerColor = meshRenderer.material.color;
            }
        }

        void Update()
        {
            if(!isLocalPlayer)
            {
                return;
            }
    
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
    
            if(m_characterController.isGrounded && Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }
            
            if(Input.GetButtonDown("Fire1"))
            {
                CmdRayCastForward();
            }
        }
    
        [Command]
        void CmdRayCastForward()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit rayHit;

            if(Physics.Raycast(ray, out rayHit, 100.0f))
            {
                Debug.Log("Hit: " + rayHit.collider.name);
                if(rayHit.collider.tag == "Pillar")
                {
                    PillarLogic pillarLogic = rayHit.collider.GetComponent<PillarLogic>();
                    if(pillarLogic)
                    {
                        pillarLogic.RpcSetColor(_playerColor);
                    }
                }
            }
        }
        
        void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
    
            if (_jump)
            {
                _heightMovement.y = JUMP_HEIGHT;
                _jump = false;
            }
    
            _heightMovement.y -= GRAVITY * Time.deltaTime;
    
            _movementInput = new Vector3(_horizontalInput, 0, _verticalInput);
            _movement = _movementInput * MOVEMENT_SPEED * Time.deltaTime;
    
            if(m_characterController)
            {
                m_characterController.Move(_movement + _heightMovement);
            }
        }
    
        public bool IsLocalPlayer()
        {
            return isLocalPlayer;
        }
}
