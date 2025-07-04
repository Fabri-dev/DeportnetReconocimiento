﻿using DeportNetReconocimiento.Api.BD;
using DeportNetReconocimiento.Api.Data.Domain;
using DeportNetReconocimiento.Properties;
using DeportNetReconocimiento.Utils;

namespace DeportNetReconocimiento.GUI
{
    public partial class WFSeleccionarUsuario : Form
    {
        private readonly BdContext _contextBd;
        private string? nombreSucursal;
        private string? idSucursal;
        private bool ingresoSinEmpleado;
        private List<Empleado> listadoEmpleados;

        public WFSeleccionarUsuario()
        {
            InitializeComponent();
            _contextBd = BdContext.CrearContexto();
            listadoEmpleados = new List<Empleado>();
            nombreSucursal = null;
            idSucursal = null;
            ingresoSinEmpleado = false;
        }
        private string ObtenerNombreSucursal()
        {
            string? nombre = ConfiguracionGeneralUtils.ObtenerConfiguracionGeneral().NombreSucursal;

            if (nombre == null)
            {
                Console.WriteLine("No se pudo obtener el nombre de la sucursal en WFSeleccionarUsuario");
                return "Nombre Gimnasio";
            }

            return nombre;
        }

        private string? ObtenerIdSucursal()
        {
            string? idSucursal = CredencialesUtils.LeerCredencialEspecifica(4);

            if (idSucursal == null)
            {
                Console.WriteLine("No se pudo obtener el id de la sucursal en WFSeleccionarUsuario");
            }

            return idSucursal;
        }

        private List<Empleado> ObtenerListadoDeEmpleados()
        {
            List<Empleado> listadoAux = _contextBd.Empleados.ToList();

            bool parseExito = int.TryParse(idSucursal, out int idSucursalInt);

            if (!parseExito)
            {
                Console.WriteLine("No se pudo parsear el id de la sucursal en WFSeleccionarUsuario");
                return listadoAux;
            }


            if (listadoAux.Count == 0)
            {
                listadoAux.Add(new Empleado(idSucursalInt, "Empleado", "Predeterminado", "", "T", "F"));

                listadoAux.First().Id = -1;

                Console.WriteLine("No se encontraron empleados en WFSeleccionarUsuario");
            }

            return listadoAux;
        }

        private void WFSeleccionarUsuario_Load(object sender, EventArgs e)
        {
            idSucursal = ObtenerIdSucursal();
            nombreSucursal = ObtenerNombreSucursal();
            listadoEmpleados = ObtenerListadoDeEmpleados();
            label1.Text = "Seleccione un usuario:";
            labelNombreSucursal.Text = nombreSucursal;
            panel1.Hide();
            CargarCombobox();
        }

        private void CargarCombobox()
        {
            //si no tiene empleados, se agrega un empleado predeterminado para que pueda pasar igual

            comboBox1.DataSource = listadoEmpleados;

            comboBox1.DisplayMember = "FullName";

            comboBox1.ValueMember = "Id";


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //limpiamos
            textBoxContrasenia.Clear();
            mensajeErrorLabel.Hide();

            //obtenemos el empleado seleccionado
            Empleado? empleadoSeleccionado = (Empleado?)comboBox1.SelectedItem;

            if (empleadoSeleccionado == null)
            {
                Console.WriteLine("Empleado seleccionado es null");
                return;
            }

            if (empleadoSeleccionado.Id == -1)
            {
                Console.WriteLine("Empleado predeterminado seleccionado");
                ingresoSinEmpleado = true;
                button1.Text = "Ingresar sin empleado";
                panel1.Hide();
                return;
            }

            ingresoSinEmpleado = false;
            label2.Text = "Ingrese la contraseña de " + empleadoSeleccionado.FullName + ":";


            panel1.Show();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (mensajeErrorLabel.Visible)
            {
                mensajeErrorLabel.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            IngresarSinEmpleado();
        }

        private void IngresarSinEmpleado()
        {
            Console.WriteLine("Logica ingresar sin empleado");
            this.Hide();
            WFPrincipal.ObtenerInstancia.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidarUsuario();
        }
        private void ApretarEnterContrasenia(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ValidarUsuario();
            }
        }
        private void ValidarUsuario()
        {
            Empleado? empleadoSeleccionado = (Empleado?)comboBox1.SelectedItem;

            if (empleadoSeleccionado == null)
            {
                Console.WriteLine("Empleado seleccionado es null");
                return;
            }

            //logica si la lista viene vacia (empleado predeterminado)
            if (ingresoSinEmpleado)
            {
                IngresarSinEmpleado();
                return;
            }

            if (empleadoSeleccionado.Password != textBoxContrasenia.Text)
            {
                mensajeErrorLabel.Show();
                mensajeErrorLabel.Text = "Contraseña incorrecta";
                return;
            }

            Credenciales? credencialesActuales = CredencialesUtils.LeerCredencialesBd();

            if (credencialesActuales == null)
            {
                Console.WriteLine("Credenciales actuales son null");
                return;
            }
            credencialesActuales.CurrentCompanyMemberId = empleadoSeleccionado.CompanyMemberId.ToString();

            CredencialesUtils.EscribirCredencialesBd(credencialesActuales);


            //si la contrasenia es correcta abrir el formulario principal
            this.Hide();
            WFPrincipal.ObtenerInstancia.Show();
        }

        private void BotonVer1_Click(object sender, EventArgs e)
        {
            if (textBoxContrasenia.UseSystemPasswordChar)
            {
                Console.WriteLine("Entro aca");
                textBoxContrasenia.UseSystemPasswordChar = false;
                BotonVer1.Image = Resources.hidden1;
            }
            else
            {
                Console.WriteLine("Entro aca X2");
                textBoxContrasenia.UseSystemPasswordChar = true;
                BotonVer1.Image = Resources.eye1;
            }
        }


    }
}
