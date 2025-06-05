# Installation

## In the form of a unity module

Installation as a unity module via a git link in the PackageManager or direct editing of `Packages/manifest.json` is supported:

```
"com.portaldweller.ecslite.unity.ugui": "https://github.com/PortalDweller/ecslite-unity-ugui.git",
```

By default, the latest release version is used. If you need a version "in development" with the latest changes, you should switch to the `develop` branch:

```
"com.portaldweller.ecslite.unity.ugui": "https://github.com/PortalDweller/ecslite-unity-ugui.git#develop",
```

## As source

The code can also be cloned or obtained as an archive from the releases page.

# Classes

## EcsUguiEmitter

`EcsUiEmitter` is a `MonoBehaviour` class responsible for generating ECS ​​events based on uGui events (press, release, drag, etc).
Must be placed on the root `GameObject` of the UI hierarchy (or at least on the root `Canvas`) and connected to the ECS infrastructure via the inspector:

```csharp
using Leopotam.EcsLite.Unity.Ugui;

public class Startup : MonoBehaviour
{
    // The field must be initialized in the inspector using the Unity editor.
    [SerializeField] EcsUguiEmitter _uguiEmitter;

    IEcsSystems _systems;

    void Start ()
    {
        _systems = new EcsSystems (new EcsWorld ());
        _systems
            .Add (new Test1System ())
            .Add (new Test2System ())
            // This call must be placed after all systems
            // which have a dependency on uGui events.
            .InjectUgui (_uguiEmitter)
            .Init ();
    }
    
    void Update ()
    {
        _systems?.Update ();
    }
    
    void OnDestroy ()
    {
        if (_systems != null) 
        {
            _systems.GetWorld ("ugui-events").Destroy ();
            _systems.Destroy ();
            _systems.GetWorld ().Destroy ();
            _systems = null;
        }
    }
}

public class Test1System : IEcsInitSystem
{
    // This field will be automatically initialized
    // a link to the emitter instance on the stage.
    readonly EcsCustomInject<EcsFromEmitter> _ugui = default;
    
    GameObject _btnGo;
    Transform _btnTransform;
    Button _btn;

    public void Init (IEcsSystems systems)
    {
        // Get a link to a widget action named "MyButton".
        _btnGo = _ugui.GetNamedObject("MyButton");
        // Read the Transform component from it.
        _btnTransform = _ugui.GetNamedObject ("MyButton").GetComponent<Transform> ();
        // Read the Button component from it.
        _btn = _ugui.GetNamedObject ("MyButton").GetComponent<Button> ();
    }
}
```

The example above can be simplified with `[EcsUguiNamedAttribute]`:

```csharp
using Leopotam.EcsLite.Unity.Ugui;

public class Test2System : IEcsInitSystem
{
    // All fields will be automatically filled with links
    // to the corresponding components from the named action widget.
    [EcsUguiNamed("MyButton")] GameObject _btnGo;
    [EcsUguiNamed("MyButton")] Transform _btnTransform;
    [EcsUguiNamed("MyButton")] Button _btn;

    public void Init (IEcsSystems systems)
    {
        // All fields are initialized and can be used here.
    }
}
```

## EcsUguiCallbackSystem

This system allows you to directly subscribe to uGui events without additional code:

```csharp
using Leopotam.EcsLite.Unity.Ugui;

public class TestUguiClickEventSystem : EcsUguiCallbackSystem
{
    [Preserve] // This attribute is required to preserve this method for il2cpp.
    [EcsUguiClickEvent]
    void OnAnyClick (in EcsUguiClickEvent evt)
    {
        Debug.Log ("Im clicked!", evt.Sender);
    }
    
    // This method will be called when clicking on a widget with an action named "exit-button".
    [Preserve]
    [EcsUguiClickEvent("exit-button")]
    void OnExitButtonClicked (in EcsUguiClickEvent evt)
    {
        Debug.Log ("exit-button clicked!", evt.Sender);
    }
}
```

List of supported action attributes (uGui events):

```csharp
[EcsUguiClickEvent]
[EcsUguiUpEvent]
[EcsUguiDownEvent]
[EcsUguiDragStartEvent]
[EcsUguiDragMoveEvent]
[EcsUguiDragEndEvent]
[EcsUguiEnterEvent]
[EcsUguiExitEvent]
[EcsUguiScrollViewEvent]
[EcsUguiSliderChangeEvent]
[EcsUguiTmpDropdownChangeEvent]
[EcsUguiTmpInputChangeEvent]
[EcsUguiTmpInputEndEvent]
[EcsUguiDropEvent]
// events added in this fork
[EcsUguiCancelEvent]
[EcsUguiClickOrSubmitEvent]
[EcsUguiSelectEvent]
[EcsUguiSubmitEvent]
```

## Actions

Actions (classes `xxxAction`) are `MonoBehaviour` components that listen for uGui widget events, look up `EcsUiEmitter` up the hierarchy, and cause the appropriate events to be generated for the ECS world.

## Components

ECS components describing events: `EcsUguiClickEvent`, `EcsUguiBeginDragEvent`, `EcsUguiEndDragEvent`, etc. - they are all standard ECS components and can be filtered with `EcsFilter`:

```csharp
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

public class TestUguiClickEventSystem : IEcsInitSystem, IEcsRunSystem
{
    EcsPool<EcsUguiClickEvent> _clickEventsPool;
    EcsFilter _clickEvents;
    
    public void Init (IEcsSystems systems) 
    {
        var world = systems.GetWorld ();
        _clickEventsPool = world.GetPool<EcsUguiClickEvent> (); 
        _clickEvents = world.Filter<EcsUguiClickEvent> ().End ();
    }

    public void Run (IEcsSystems systems) 
    {
        foreach (var entity in _clickEvents) 
        {
            ref EcsUguiClickEvent data = ref _clickEventsPool.Get (entity);
            Debug.Log ("Im clicked!", data.Sender);
        }
    }
}
```