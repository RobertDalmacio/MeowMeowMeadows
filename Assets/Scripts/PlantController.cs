using Animancer;
using UnityEngine;

public class PlantController : MonoBehaviour {
    [SerializeField] private AnimancerComponent _Animancer;
    [SerializeField] private AnimationClip _Clip;

    public float clipLength = 0f;
    protected virtual void OnEnable() {
        if (_Animancer == null) {
            _Animancer = GetComponent<AnimancerComponent>();
        }
        clipLength = _Clip.length;

        AnimancerState state = _Animancer.Play(_Clip);

        state.Events(this).OnEnd = OnAnimationEnd;
    }

    public void PauseClip() {
        AnimancerState state = _Animancer.States[_Clip];
        state.IsPlaying = false;
    }


    private void OnAnimationEnd() {
        Debug.Log("Animation finished");
        PauseClip();
    }
}
