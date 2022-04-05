#if USING_FIREBASE_AUTH
using System;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace Truongtv.Services.Firebase
{
    public class FirebaseAuthController : MonoBehaviour
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;

        private void Start()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _user = _auth.CurrentUser;
        }

        public bool IsFirebaseUserExist()
        {
            return _user == null;
        }
        public void CreateAccountWithEmailAndPassword(string email,string password,Action<bool> callback=null)
        {
            _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    callback?.Invoke(false);
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    callback?.Invoke(false);
                    return;
                }
                _user = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    _user.DisplayName, _user.UserId);
                callback?.Invoke(true);
            });
        }



        public void SignInWithCredential(Credential credential,Action<bool> callback=null)
        {
            _auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    callback?.Invoke(false);
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    callback?.Invoke(false);
                    return;
                }

                _user = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    _user.DisplayName, _user.UserId);
                callback?.Invoke(true);
            });
        }

        public void LinkWithCredential(Credential credential, Action<bool> callback = null)
        {
            _auth.CurrentUser.LinkAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted) {
                    // Link Success
                    callback?.Invoke(true);
                } else
                {
                    if (task.Exception == null) return;
                    foreach (var exception in task.Exception.Flatten().InnerExceptions) {
                        var firebaseEx =
                            exception as FirebaseAccountLinkException;
                        if (firebaseEx != null && firebaseEx.UserInfo.UpdatedCredential.IsValid()) {
                            // Attempt to sign in with the updated credential.
                            SignInWithCredential(firebaseEx.UserInfo.UpdatedCredential,callback);
                        } else {
                            Debug.LogError("Link with credential failed:" + firebaseEx );
                            callback?.Invoke(false);
                        }
                    } 
                }
            });
        }

        public void SignOut()
        {
            _auth.SignOut();
        }
        public void DeleteUser()
        {
            _user = _auth.CurrentUser;
            _user?.DeleteAsync().ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User deleted successfully.");
            });
        }
    }
}
#endif