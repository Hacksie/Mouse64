using UnityEngine;

namespace HackedDesign
{
    public class TutorialMissionSelectState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;
        private UI.DialogPresenter dialogPresenter;
        private UI.MissionPresenter missionPresenter;
        private UI.LevelPresenter levelPresenter;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public TutorialMissionSelectState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.DialogPresenter dialogPresenter, UI.MissionPresenter missionPresenter, UI.LevelPresenter levelPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.dialogPresenter = dialogPresenter;
            this.missionPresenter = missionPresenter;
            this.levelPresenter = levelPresenter;
        }

        public void Begin()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.Reset();
            //GameManager.Instance.Data.currentLevel = levels[Data.currentLevelIndex];
            //this.player.Sit = true;
            this.player.transform.position = new Vector3(-0.85f, 0.275f, 0);
            this.pool.DestroyEntities();
            //GameManager.Instance.Data.currentLevel.name = "hotdog";
            this.levelRenderer.LoadMissionTutorialLevel();
            AudioManager.Instance.PlayMenuMusic();
            GameManager.Instance.ParticlesSelect.Play();
            GameManager.Instance.Data.currentLevel.currentDialogue = GameManager.Instance.Data.currentLevel.settings.startingDialogue;
            GameManager.Instance.Data.currentLevel.currentDialogueIndex = 0;
            ShowDialog();
        }

        public void End()
        {
            GameManager.Instance.ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            this.player.Sit = false;
            this.missionPresenter.Hide();
            EndDialog();
        }

        public void Update()
        {
            /*
            if (GameManager.Instance.Data.currentLevel.currentDialogueIndex >= GameManager.Instance.Data.currentLevel.dialogue.Count)
            {
                this.dialogPresenter.Hide();
                this.missionPresenter.Show();
                this.missionPresenter.Repaint();
                this.levelPresenter.Hide();
            }
            else
            {

                this.levelPresenter.Show();
                this.missionPresenter.Hide();
                this.levelPresenter.Repaint();
            }*/

            this.player.UpdateBehavior();
        }

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            foreach (var e in this.pool.Pool)
            {
                e.UpdateLateBehaviour();
            }
        }

        public void Start()
        {

        }

        public void ShowDialog()
        {
            this.dialogPresenter.Show();
            this.dialogPresenter.Repaint();
        }

        public void EndDialog()
        {
            this.dialogPresenter.Hide();
        }
    }
}