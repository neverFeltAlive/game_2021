// GENERATED AUTOMATICALLY FROM 'Assets/Input System/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Main Controls"",
            ""id"": ""dfc5b4a5-c5a4-4022-b2c4-f16b66fae66c"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""bff56714-c95a-42e9-ae29-cd741338af72"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""726ca37e-0d29-483b-a5f3-7ddbfc6d9b52"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9716aa27-71d0-46aa-bd9e-1ba62dd985cb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Press""
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""be68ddc5-9e60-40d6-9ead-d16f4c8d1013"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Press""
                },
                {
                    ""name"": ""Dash Return"",
                    ""type"": ""Button"",
                    ""id"": ""5cc8a01f-52c3-4274-8662-9436e67a1ede"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Track"",
                    ""type"": ""Button"",
                    ""id"": ""64ffd601-0fbb-484d-bce3-61781e4de389"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.75)""
                },
                {
                    ""name"": ""OverLoad"",
                    ""type"": ""Button"",
                    ""id"": ""45eb2b92-a57c-4c27-b54b-c1c2fee76dc3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Toggle Shooting"",
                    ""type"": ""Button"",
                    ""id"": ""3defced4-0f57-4748-86b4-8738826d958c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""24c7558f-2d2c-4e8e-9737-769b592f9ea7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""233fea56-f609-4187-aca0-e8e6229283f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle Lights"",
                    ""type"": ""Button"",
                    ""id"": ""35e150a2-a405-40bc-a4a4-f871f5da9318"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.75)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""700d207e-911c-477e-931a-307a119ebb95"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b685b522-d02f-49e8-8f00-1b6a9801fb78"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""342c98f0-c495-4958-8b18-ad2b93b9859e"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be86d78c-f6e7-46eb-8759-2df74499e302"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0717e7af-5eaa-4ae8-80e4-b15e2cb92da5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Track"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""036cc2ed-639d-429b-957a-c1dda2180a5b"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OverLoad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f695fc2a-1fee-441a-b05e-d72888e55c53"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e81733a-3cd6-4350-a232-1ec1c13679a5"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14f23c2b-d85f-449c-9a27-8d2ea16afed2"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Toggle Shooting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78f63652-954b-4bf5-8f93-ad70687453ed"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Toggle Lights"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91582631-8ea0-4969-8fa3-77d858c21da0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main Controls
        m_MainControls = asset.FindActionMap("Main Controls", throwIfNotFound: true);
        m_MainControls_Walk = m_MainControls.FindAction("Walk", throwIfNotFound: true);
        m_MainControls_Roll = m_MainControls.FindAction("Roll", throwIfNotFound: true);
        m_MainControls_Attack = m_MainControls.FindAction("Attack", throwIfNotFound: true);
        m_MainControls_Dash = m_MainControls.FindAction("Dash", throwIfNotFound: true);
        m_MainControls_DashReturn = m_MainControls.FindAction("Dash Return", throwIfNotFound: true);
        m_MainControls_Track = m_MainControls.FindAction("Track", throwIfNotFound: true);
        m_MainControls_OverLoad = m_MainControls.FindAction("OverLoad", throwIfNotFound: true);
        m_MainControls_ToggleShooting = m_MainControls.FindAction("Toggle Shooting", throwIfNotFound: true);
        m_MainControls_Aim = m_MainControls.FindAction("Aim", throwIfNotFound: true);
        m_MainControls_Shoot = m_MainControls.FindAction("Shoot", throwIfNotFound: true);
        m_MainControls_ToggleLights = m_MainControls.FindAction("Toggle Lights", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Main Controls
    private readonly InputActionMap m_MainControls;
    private IMainControlsActions m_MainControlsActionsCallbackInterface;
    private readonly InputAction m_MainControls_Walk;
    private readonly InputAction m_MainControls_Roll;
    private readonly InputAction m_MainControls_Attack;
    private readonly InputAction m_MainControls_Dash;
    private readonly InputAction m_MainControls_DashReturn;
    private readonly InputAction m_MainControls_Track;
    private readonly InputAction m_MainControls_OverLoad;
    private readonly InputAction m_MainControls_ToggleShooting;
    private readonly InputAction m_MainControls_Aim;
    private readonly InputAction m_MainControls_Shoot;
    private readonly InputAction m_MainControls_ToggleLights;
    public struct MainControlsActions
    {
        private @PlayerControls m_Wrapper;
        public MainControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_MainControls_Walk;
        public InputAction @Roll => m_Wrapper.m_MainControls_Roll;
        public InputAction @Attack => m_Wrapper.m_MainControls_Attack;
        public InputAction @Dash => m_Wrapper.m_MainControls_Dash;
        public InputAction @GenericDashAndReturn => m_Wrapper.m_MainControls_DashReturn;
        public InputAction @Track => m_Wrapper.m_MainControls_Track;
        public InputAction @OverLoad => m_Wrapper.m_MainControls_OverLoad;
        public InputAction @ToggleShooting => m_Wrapper.m_MainControls_ToggleShooting;
        public InputAction @Aim => m_Wrapper.m_MainControls_Aim;
        public InputAction @Shoot => m_Wrapper.m_MainControls_Shoot;
        public InputAction @ToggleLights => m_Wrapper.m_MainControls_ToggleLights;
        public InputActionMap Get() { return m_Wrapper.m_MainControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMainControlsActions instance)
        {
            if (m_Wrapper.m_MainControlsActionsCallbackInterface != null)
            {
                @Walk.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnWalk;
                @Roll.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnRoll;
                @Attack.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAttack;
                @Dash.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDash;
                @GenericDashAndReturn.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDashReturn;
                @GenericDashAndReturn.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDashReturn;
                @GenericDashAndReturn.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnDashReturn;
                @Track.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnTrack;
                @Track.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnTrack;
                @Track.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnTrack;
                @OverLoad.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnOverLoad;
                @OverLoad.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnOverLoad;
                @OverLoad.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnOverLoad;
                @ToggleShooting.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleShooting;
                @ToggleShooting.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleShooting;
                @ToggleShooting.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleShooting;
                @Aim.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnShoot;
                @ToggleLights.started -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleLights;
                @ToggleLights.performed -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleLights;
                @ToggleLights.canceled -= m_Wrapper.m_MainControlsActionsCallbackInterface.OnToggleLights;
            }
            m_Wrapper.m_MainControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @GenericDashAndReturn.started += instance.OnDashReturn;
                @GenericDashAndReturn.performed += instance.OnDashReturn;
                @GenericDashAndReturn.canceled += instance.OnDashReturn;
                @Track.started += instance.OnTrack;
                @Track.performed += instance.OnTrack;
                @Track.canceled += instance.OnTrack;
                @OverLoad.started += instance.OnOverLoad;
                @OverLoad.performed += instance.OnOverLoad;
                @OverLoad.canceled += instance.OnOverLoad;
                @ToggleShooting.started += instance.OnToggleShooting;
                @ToggleShooting.performed += instance.OnToggleShooting;
                @ToggleShooting.canceled += instance.OnToggleShooting;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @ToggleLights.started += instance.OnToggleLights;
                @ToggleLights.performed += instance.OnToggleLights;
                @ToggleLights.canceled += instance.OnToggleLights;
            }
        }
    }
    public MainControlsActions @MainControls => new MainControlsActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IMainControlsActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnDashReturn(InputAction.CallbackContext context);
        void OnTrack(InputAction.CallbackContext context);
        void OnOverLoad(InputAction.CallbackContext context);
        void OnToggleShooting(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnToggleLights(InputAction.CallbackContext context);
    }
}
