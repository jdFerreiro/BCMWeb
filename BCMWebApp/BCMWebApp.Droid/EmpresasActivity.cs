using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace BCMWebApp.Droid
{
    [Activity(Label = "BCMWeb - Empresas", MainLauncher = false, SupportsPictureInPicture = true)]
    public class EmpresasActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Empresas);

            GridLayout contenedor = FindViewById<GridLayout>(Resource.Id.gridEmpresas);
            decimal _result = GlobalVars.BotonEmpresa.Count / 2;
            int _rows = int.Parse(Math.Ceiling(_result).ToString());
            contenedor.RowCount = _rows;

            GridLayout.LayoutParams _params = new GridLayout.LayoutParams();
            _params.Width = 200;
            _params.Height = 300;

            foreach (Empresa _empresa in GlobalVars.BotonEmpresa)
            {

                Button empButton = new Button(this)
                {
                    Id = int.Parse(_empresa.Id.ToString()),
                    Text = _empresa.Nombre,
                };
                empButton.SetHeight(200);
                empButton.SetWidth(250);

                contenedor.AddView(empButton);
            }
        }
    }
}