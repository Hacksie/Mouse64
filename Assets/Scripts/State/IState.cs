namespace HackedDesign
{
    public interface IState
    {
        void Begin();
        void Update();
        void LateUpdate();
        void FixedUpdate();
        void End();
        void Interact();
        void Start();
        void Select();

        bool PlayerActionAllowed { get; }
    }   
}