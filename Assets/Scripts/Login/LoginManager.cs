using DG.Tweening;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

namespace BGS.Game
{
    public class LoginManager
    {
        protected LoginHandler Handler;

        #region Handlers
        protected void LoginSceneInitEventHandler(LoginSceneInitEvent e)
        {
            //AskPermissions();
        }

        protected void SwitchLoginStateEventHandler(SwitchLoginStateEvent e)
        {
            if (e.Exsisting)
            {
                MonoHelper.Instance.PrintMessage("user exsisting", "blue");
                Handler.LoadingPrefab.SetActive(true);
                Handler.LoginPrefab.SetActive(false);
                Handler.LoadingSlider.DOValue(1f, 1f).OnComplete(() => { 
                   SceneManager.LoadSceneAsync(1); 
                });
            }

            else
            {
                MonoHelper.Instance.PrintMessage("user doesn't exsists", "red");
                Handler.LoadingPrefab.SetActive(false);
                Handler.LoginPrefab.SetActive(true);
            }
        }

        #endregion
        
        
        #region Methods

        private void AskPermissions()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        }
        #endregion
    }
}
