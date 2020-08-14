using UnityEngine;

namespace HackedDesign
{
    public class MissionCompleteState : IState
    {
        private PlayerController player;
        private UI.AbstractPresenter missionCompletePresenter;

        public bool PlayerActionAllowed => false;

        public MissionCompleteState(PlayerController player, UI.AbstractPresenter missionCompletePresenter)
        {
            this.player = player;
            this.missionCompletePresenter = missionCompletePresenter;
        }

        public void Begin()
        {
            this.player.Stop();
            CalcScore();
            AudioManager.Instance.PlayMissionSuccessMusic();
            this.missionCompletePresenter.Show();
            this.missionCompletePresenter.Repaint();
        }

        public void End()
        {
            this.missionCompletePresenter.Hide();
        }

        private void CalcScore()
        {
            
            var time = (GameManager.Instance.Data.currentLevel.window - GameManager.Instance.Data.timer);
            var score = Mathf.Max(0, 1000 + (Mathf.FloorToInt(time) * 20) + (GameManager.Instance.Data.alert * -50) + (5 - (GameManager.Instance.Data.bullets * -50)) + (GameManager.Instance.Data.currentLevel.reactions * -25));

            Logger.Log("Mission complete", "calc score ", score.ToString());

            GameManager.Instance.Data.currentLevel.score = score;
            GameManager.Instance.Data.score += score;
        }

        public void Update()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            
        }

        public void Interact()
        {
            
        }

        public void Select()
        {
            
        }

        public void Start()
        {
            
        }
    }
}