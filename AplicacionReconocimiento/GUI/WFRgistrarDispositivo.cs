﻿using DeportNetReconocimiento.Api.Data.Domain;
using DeportNetReconocimiento.Api.Services;
using DeportNetReconocimiento.GUI;
using DeportNetReconocimiento.Hikvision.SDKHikvision;
using DeportNetReconocimiento.Properties;
using DeportNetReconocimiento.Utils;

namespace DeportNetReconocimiento
{
    public partial class WFRgistrarDispositivo : Form
    {
        public bool ignorarCerrarPrograma = false;
        private static WFRgistrarDispositivo? instancia;
        private bool inputsValidos = false;
        private Loading loading;
        public Credenciales? credenciales;
        private bool seClickeoBotonLogin = false;
        private bool guardarCambios = false;

        //Tipo de apertura 0 = normal, 1 = Credenciales bloqueadas(solo cambiar IP), 2= Modificar credenciales ;
        public int tipoApertura { get; set; } = 0;
        private WFRgistrarDispositivo()
        {
            tipoApertura = 0;
            InitializeComponent();
            this.loading = new Loading();
        }

        private void VerificarTipoDeApertura()
        {
            switch (tipoApertura)
            {
                case 0:
                    textBoxPort.Enabled = true;
                    textBoxPassword.Enabled = true;
                    textBoxSucursalID.Enabled = true;
                    textBoxTokenSucursal.Enabled = true;
                    textBoxUserName.Enabled = true;
                    instancia.ControlBox = true;
                    instancia.MinimizeBox = true;
                    btnAdd.Text = "Login";
                    label1.Text = "Registrar Dispositivo";
                    break;
                case 1:
                    textBoxPort.Enabled = false;
                    textBoxPassword.Enabled = false;
                    textBoxSucursalID.Enabled = false;
                    textBoxTokenSucursal.Enabled = false;
                    textBoxUserName.Enabled = false;
                    instancia.ControlBox = false;
                    instancia.MinimizeBox = false;
                    btnAdd.Text = "Guardar";
                    break;
                case 2:
                    textBoxPort.Enabled = true;
                    textBoxPassword.Enabled = true;
                    textBoxSucursalID.Enabled = true;
                    textBoxTokenSucursal.Enabled = true;
                    textBoxUserName.Enabled = true;
                    instancia.ControlBox = true;
                    instancia.MinimizeBox = true;
                    btnAdd.Text = "Guardar";
                    label1.Text = "Actualizar credenciales";
                    break;
            }
        }


        private void AgregarValoresAInputs(Credenciales credenciales)
        {
            //ip , puerto, usuario, contraseña, sucursalId, tokenSucursal
            textBoxDeviceAddress.Text = credenciales.Ip;
            textBoxPort.Text = credenciales.Port;
            textBoxUserName.Text = credenciales.Username;
            textBoxPassword.Text = credenciales.Password;
            textBoxSucursalID.Text = credenciales.BranchId;
            textBoxTokenSucursal.Text = credenciales.BranchToken;
        }

        private void WFRgistrarDispositivo_Load(object sender, EventArgs e)
        {
            VerificarTipoDeApertura();
            if (tipoApertura != 0)
            {
                Credenciales? credenciales = CredencialesUtils.LeerCredencialesBd();

                if (credenciales != null)
                {
                    AgregarValoresAInputs(credenciales);
                    ignorarCerrarPrograma = true;

                }
            }
        }


        public static WFRgistrarDispositivo ObtenerInstancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new WFRgistrarDispositivo();
                }
                return instancia;
            }
        }


        private bool ValidarInputs(bool buscarDisp = false)
        {
            bool flag = false;

            //si queremos buscar el disp, podemos obviar estas validaciones
            if (!buscarDisp)
            {
                if (string.IsNullOrEmpty(textBoxDeviceAddress.Text) ||
                    string.IsNullOrWhiteSpace(textBoxDeviceAddress.Text)
                    || textBoxDeviceAddress.Text.Length > 128)
                {
                    //Properties.Resources.deviceAddressTips
                    MessageBox.Show("Ip del dispositivo invalida");
                    return flag;
                }

                bool parseSucursalId = int.TryParse(textBoxSucursalID.Text, out int parseSucursalIdOut);

                if (!parseSucursalId)
                {
                    MessageBox.Show("El ID de la sucursal debe ser numerico");
                    return flag;
                }
                if (textBoxSucursalID.Text.Length > 12 || parseSucursalIdOut <= 0)
                {
                    MessageBox.Show("El ID de la sucursal no puede ser mayor a 12 caracteres ni negativo");
                    return flag;
                }

                if (textBoxTokenSucursal.Text.Length < 1)
                {
                    MessageBox.Show("El token de la sucursal es obligatorio");
                    return flag;
                }

            }


            bool parsePuerto = int.TryParse(textBoxPort.Text, out int parsePuertoOut);
            if (!parsePuerto)
            {
                MessageBox.Show("El puerto debe ser numerico");
                return flag;
            }

            if (textBoxPort.Text.Length > 5 || parsePuertoOut <= 0)
            {
                //Properties.Resources.portTips
                MessageBox.Show("Puerto con longitud incorrecta");
                return flag;
            }

            if (textBoxUserName.Text.Length > 32 || textBoxUserName.Text.Length < 3)
            {
                //Properties.Resources.usernameAndPasswordTips
                MessageBox.Show("Usuario no puede ser menor a 3 caracteres ni mayor a 32 caracteres");
                return flag;
            }

            if (textBoxPassword.Text.Length > 16 || textBoxPassword.Text.Length < 3)
            {
                //Properties.Resources.usernameAndPasswordTips
                MessageBox.Show("Contraseña no puede ser menor a 3 caracteres ni mayor a 16 caracteres");
                return flag;
            }

            //si pasa todos los filtros 
            flag = true;

            return flag;
        }



        private async void BtnAdd_Click(object sender, EventArgs e)
        {


            if (!ValidarInputs() || loading.Visible || seClickeoBotonLogin)
            {
                return;
            }
            seClickeoBotonLogin = true;


            Hik_Resultado resultadoLogin = Hik_Controladora_General.Instancia.InicializarPrograma(textBoxUserName.Text, textBoxPassword.Text, textBoxPort.Text, textBoxDeviceAddress.Text);

            if (!resultadoLogin.Exito)
            {
                seClickeoBotonLogin = false;

                resultadoLogin.MessageBoxResultado("Error al incializar el dispositivo Hikvision");
                return;
            }

            Hik_Resultado conexionDx = await WebServicesDeportnet.TestearConexionDeportnet(textBoxTokenSucursal.Text, textBoxSucursalID.Text);


            if (!conexionDx.Exito)
            {
                seClickeoBotonLogin = false;

                Console.WriteLine("Hubo un error de conexión con DX");
                conexionDx.MessageBoxResultado("Conexion con Deportnet");
                return;
            }


            //ip , puerto, usuario, contraseña, sucursalId, tokenSucursal
            //Escribe las credenciales en la base de datos

            credenciales = CrearCredencialesDesdeTextbox();
            CredencialesUtils.EscribirCredencialesBd(credenciales);
            WFPrincipal.ObtenerInstancia.ReactivarTimer();


            //Creador de credenciales manual
            //CredencialesUtils.EscribirArchivoCredenciales(["192.168.0.203", "8000", "admin", "facu", "321", "laskjdhf"]);

            ignorarCerrarPrograma = true;
            seClickeoBotonLogin = false;
            guardarCambios = true;
            this.Close();
        }

        private Credenciales CrearCredencialesDesdeTextbox()
        {
            //Credenciales de prueba

            return new Credenciales(
            textBoxDeviceAddress.Text,
            textBoxPort.Text,
            textBoxUserName.Text,
            textBoxPassword.Text,
            textBoxSucursalID.Text,
            textBoxTokenSucursal.Text,
            null);

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void CerrarFormulario(object sender, FormClosingEventArgs e)
        {
            if (!ignorarCerrarPrograma)
            {

                var result = MessageBox.Show("¿Estás seguro de que quieres cerrar la aplicación?",
                                             "Confirmación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cerrar completamente la aplicación
                    Environment.Exit(0); // 0 indica salida exitosa; otro valor indica error.
                }
                else
                {
                    // Cancelar el cierre
                    e.Cancel = true;
                }
            }

            if (tipoApertura == 1)
            {
                var result = MessageBox.Show("Cerrar la aplicación para volver a inicializar la conexión",
                                            "Confirmación",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cerrar completamente la aplicación
                    Environment.Exit(0); // 0 indica salida exitosa; otro valor indica error.
                }
                else
                {
                    // Cancelar el cierre
                    e.Cancel = true;
                }
            }

            if (tipoApertura == 2)
            {
                if (guardarCambios)
                {
                    MessageBox.Show("Credenciales actualizadas con exito, por favor vuelva a iniciar el gestor de acceso", "Credenciales actualizadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.UseSystemPasswordChar)
            {
                textBoxPassword.UseSystemPasswordChar = false;
                BotonVer1.Image = Resources.hidden1;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
                BotonVer1.Image = Resources.eye1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxSucursalID.UseSystemPasswordChar)
            {
                textBoxSucursalID.UseSystemPasswordChar = false;
                BotonVer2.Image = Resources.hidden1;
            }
            else
            {
                textBoxSucursalID.UseSystemPasswordChar = true;
                BotonVer2.Image = Resources.eye1;
            }

        }

        private void BotonVer3_Click(object sender, EventArgs e)
        {
            if (textBoxTokenSucursal.UseSystemPasswordChar)
            {
                textBoxTokenSucursal.UseSystemPasswordChar = false;
                BotonVer3.Image = Resources.hidden1;
            }
            else
            {
                textBoxTokenSucursal.UseSystemPasswordChar = true;
                BotonVer3.Image = Resources.eye1;
            }
        }



        private async void botonBuscarIp_Click(object sender, EventArgs e)
        {
            bool buscarDisp = true;
            if (!ValidarInputs(buscarDisp) || loading.Visible)
            {
                return;
            }
            loading = new Loading();
            loading.Show();

            Hik_Resultado resultadoLogin = await Task.Run(() => BuscadorIpDispositivo.ObtenerIpDispositivo(textBoxPort.Text, textBoxUserName.Text, textBoxPassword.Text));

            loading.Close();


            if (!resultadoLogin.Exito)
            {
                resultadoLogin.MessageBoxResultado("Error al incializar el dispositivo Hikvision");
                return;
            }

            textBoxDeviceAddress.Text = resultadoLogin.Mensaje;
        }
    }
}
