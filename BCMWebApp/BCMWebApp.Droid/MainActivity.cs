using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Net;

namespace BCMWebApp.Droid
{
    [Activity(Label = "BCMWeb", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var Indicator = new ActivityIndicator
            // Get our button from the layout resource,
            // and attach an event to it
            //var imageBitmap = GetImageBitmapFromUrl("https://www.bcmweb.net/content/images/Logo-BiaPlus.png");
            //ImageView _ivLogo = FindViewById<ImageView>(Resource.Id.LogoView);
            //_ivLogo.SetImageBitmap(imageBitmap);

            GlobalVars.Clear();

            Button login = FindViewById<Button>(Resource.Id.botLogin);
            login.Click += Login_Click;

        }

        private async void Login_Click(object sender, EventArgs e)
        {
            EditText etUsuario = FindViewById<EditText>(Resource.Id.etUsuario);
            EditText etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            TextView tvErrorEmail = FindViewById<TextView>(Resource.Id.tvUsuarioErrorLabel);
            TextView tvErrorPassword = FindViewById<TextView>(Resource.Id.tvPasswordErrorLabel);
            TextView tvErrorGeneral = FindViewById<TextView>(Resource.Id.tvGeneralErrorLabel);
            tvErrorEmail.Text = string.Empty;
            tvErrorPassword.Text = string.Empty;
            tvErrorGeneral.Text = string.Empty;

            bool IsValid = true;

            if (string.IsNullOrEmpty(etUsuario.Text))
            {
                etUsuario.SetBackgroundResource(Resource.Drawable.normalRedBorder);
                tvErrorEmail.Text = GetString(Resource.String.emailRequired);
                IsValid = false;
            }
            if (!IsValidEmail(etUsuario.Text))
            {
                etUsuario.SetBackgroundResource(Resource.Drawable.normalRedBorder);
                tvErrorEmail.Text = GetString(Resource.String.emailInvalid);
                IsValid = false;
            }
            if (string.IsNullOrEmpty(etPassword.Text))
            {
                etPassword.SetBackgroundResource(Resource.Drawable.normalRedBorder);
                tvErrorPassword.Text = GetString(Resource.String.passwordRequired);
                IsValid = false;
            }

            if (IsValid)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
                bool done = await DataService.getToken(etUsuario.Text, etPassword.Text);
                if (!done)
                {
                    tvErrorGeneral.Text = GetString(Resource.String.loginError1);
                    return;
                }

                string _userUrl = string.Format("https://www.bcmweb.net/api/Usuario/GetByCredentials/{0}/{1}", etUsuario.Text, etPassword.Text);
                done = await DataService.getdataUsuario(_userUrl);

                if (!done)
                {
                    tvErrorGeneral.Text = GetString(Resource.String.ErrorConexion);
                    return;
                }

                string _EmpresasUrl = string.Format("https://www.bcmweb.net/api/empresa/getbyuser/{0}", GlobalVars.UserId.ToString());
                done = await DataService.getEmpresasUsuario(_EmpresasUrl);
                if (!done)
                {
                    tvErrorGeneral.Text = GetString(Resource.String.ErrorConexion);
                    return;
                }


                var Empresas = new Intent(this, typeof(EmpresasActivity));
                Empresas.PutExtra("FirstData", "Data from first Activity");
                StartActivity(Empresas);
            }
        }


        private bool IsValidEmail(string email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}


