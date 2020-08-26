namespace HackedDesign
{
    public interface IState
    {
        void Begin();
        void Update();
        void LateUpdate();
        void FixedUpdate();
        void End();
        void Start();
        
        bool PlayerActionAllowed { get; }
        bool Battle { get; }
    }   
}