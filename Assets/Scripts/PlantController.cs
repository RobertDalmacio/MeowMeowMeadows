using Animancer;
using UnityEngine;

public class PlantController : MonoBehaviour {
    [SerializeField] private AnimancerComponent _Animancer;
    [SerializeField] private AnimationClip _Clip;

    public bool finished = false;

    protected virtual void OnEnable() {
        if (_Animancer == null) {
            _Animancer = GetComponent<AnimancerComponent>();
        }

        AnimancerState state = _Animancer.Play(_Clip);

        state.Events(this).OnEnd = OnAnimationEnd;
    }

    private void OnAnimationEnd() {
        Debug.Log("Animation finished");
        finished = true;
    }
}
