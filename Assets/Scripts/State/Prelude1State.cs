using UnityEngine;

namespace HackedDesign
{
    public class Prelude1State : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;
        private UI.DialogPresenter dialogPresenter;

        private bool actionAllowed = false;

        public bool PlayerActionAllowed => actionAllowed;
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Prelude1State(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.DialogPresenter dialogPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.dialogPresenter = dialogPresenter;
        }

        public void Begin()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.Reset();
            this.player.transform.position = new Vector3(1.75f, 0.275f, 0);
            this.pool.DestroyEntities();
            this.levelRenderer.LoadPrelude1Level();
            AudioManager.Instance.PlayMissionSelectMusic();
            GameManager.Instance.ParticlesLeft.Play();
            GameManager.Instance.ParticlesRight.transform.position = new Vector3(18.0f, 4, 0);
            GameManager.Instance.ParticlesRight.Play();
            GameManager.Instance.Data.currentLevel.currentDialogue = GameManager.Instance.Data.currentLevel.settings.startingDialogue;
            GameManager.Instance.Data.currentLevel.currentDialogueIndex = 0;
            GameManager.Instance.LightManager.SetGlobalLight(GlobalLight.Room);
            Cursor.visible = false;
            if (!GameManager.Instance.GameSettings.skipDialog)
            {
                ShowDialog();
            }
            else {
                EndDialog();
            }
        }

        public void End()
        {
            Cursor.visible = true;
            GameManager.Instance.ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            EndDialog();
        }

        public void Update()
        {
            //FIXME: make a 'end of dialogue' state event
            /*
            if (GameManager.Instance.Data.currentLevel.currentDialogueIndex >= GameManager.Instance.Data.currentLevel.currentDialogue.text.Count)
            {
                this.dialogPresenter.Hide();
                actionAllowed = true;
            }
            else
            {
                this.dialogPresenter.Show();
                this.dialogPresenter.Repaint();
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
            //Cursor.visible = true;
            this.dialogPresenter.Show();
            this.dialogPresenter.Repaint();

        }

        public void EndDialog()
        {
            //Cursor.visible = false;
            this.dialogPresenter.Hide();
            actionAllowed = true;
        }
    }
}