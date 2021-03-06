using UnityEngine;

namespace HackedDesign
{
    public class MissionCompleteState : IState
    {
        private PlayerController player;
        private UI.AbstractPresenter missionCompletePresenter;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
            ShowDialog();
        }

        public void End()
        {
            EndDialog();
        }

        private void CalcScore()
        {
            
            var time = (GameManager.Instance.Data.currentLevel.settings.window - GameManager.Instance.Data.timer);
            var score = Mathf.Max(0, 1000 + (Mathf.FloorToInt(time) * 20) + (GameManager.Instance.Data.alert * -50) + (5 - (GameManager.Instance.Data.bullets * -50)) + (GameManager.Instance.Data.currentLevel.reactions * -25));

            Logger.Log("Mission complete", "calc score ", score.ToString(), "1000 + ", (Mathf.FloorToInt(time) * 20).ToString(), "time + ", (GameManager.Instance.Data.alert * -50).ToString(), "alerts + ", (5 - (GameManager.Instance.Data.bullets * -50)).ToString(), "bullets remaining + ", (GameManager.Instance.Data.currentLevel.reactions * -25).ToString(), "reactions" );

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

        public void Start()
        {
            
        }

        public void ShowDialog()
        {
            this.missionCompletePresenter.Show();
            this.missionCompletePresenter.Repaint();
        }

        public void EndDialog()
        {
            this.missionCompletePresenter.Hide();
        }
    }
}