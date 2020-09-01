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
        void ShowDialog();
        void EndDialog();

        //Dialog CurrentDialog { get; set; }
        
        bool PlayerActionAllowed { get; }
        bool Battle { get; }
    }   
}