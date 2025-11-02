using JapaneseRestaurant.Services;

namespace JapaneseRestaurant
{
    public partial class Form1 : Form
    {
        private Label labelWelcome;
        private Label labelUser;
        private Label labelPassword;
        private TextBox txtUser;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister; // nuevo
        private Label lblMessage;
        private readonly UserService userService = new();

        public Form1()
        {
            InitializeComponent();
            BuildLoginUi();
        }

        private void BuildLoginUi()
        {
            Controls.Clear();
            Width = 380;
            Height = 360;
            Text = "Inicio";
            labelWelcome = new Label()
            {
                Text = "Bienvenido",
                Left = 40,
                Top = 20,
                Width = 250,
                Font = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold)
            };
            labelUser = new Label() { Text = "Usuario", Left = 40, Top = 80 };
            labelPassword = new Label() { Text = "Contraseña", Left = 40, Top = 120 };
            txtUser = new TextBox() { Left = 140, Top = 76, Width = 180 };
            txtPassword = new TextBox()
            {
                Left = 140,
                Top = 116,
                Width = 180,
                UseSystemPasswordChar = true
            };
            btnLogin = new Button()
            {
                Text = "Iniciar Sesión",
                Left = 140,
                Top = 160,
                Width = 180
            };
            btnRegister = new Button()
            {
                Text = "Crear Cuenta",
                Left = 140,
                Top = 200,
                Width = 180
            };
            lblMessage = new Label()
            {
                Left = 40,
                Top = 250,
                Width = 300,
                ForeColor = Color.DarkRed
            };
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            Controls.AddRange(new Control[]
            {
                labelWelcome,
                labelUser,
                labelPassword,
                txtUser,
                txtPassword,
                btnLogin,
                btnRegister,
                lblMessage
            });
            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblMessage.Text = "";
            var user = txtUser.Text.Trim();
            var pass = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(user))
            {
                lblMessage.Text = "Ingrese usuario";
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                lblMessage.Text = "Ingrese contraseña";
                txtPassword.Focus();
                return;
            }

            if (userService.ValidateUser(user, pass))
            {
                ShowMainPanel(user);
            }
            else
            {
                lblMessage.Text = "Credenciales inválidas";
            }
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            lblMessage.ForeColor = Color.DarkRed;
            lblMessage.Text = "";
            var user = txtUser.Text.Trim();
            var pass = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(user))
            {
                lblMessage.Text = "Usuario requerido";
                txtUser.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(pass))
            {
                lblMessage.Text = "Contraseña requerida";
                txtPassword.Focus();
                return;
            }
            if (pass.Length < 3)
            {
                lblMessage.Text = "Mínimo 3 caracteres";
                txtPassword.Focus();
                return;
            }
            if (userService.RegisterUser(user, pass))
            {
                lblMessage.ForeColor = Color.DarkGreen;
                lblMessage.Text = "Usuario creado. Ahora inicie sesión.";
                txtPassword.Clear();
            }
            else
            {
                lblMessage.Text = "Usuario ya existe";
            }
        }

        private void ShowMainPanel(string user)
        {
            Controls.Clear();
            Text = "Panel Principal";
            Width = 520;
            Height = 380;
            var lbl = new Label()
            {
                Text = $"Bienvenido {user}",
                Left = 20,
                Top = 20,
                Width = 300,
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold)
            };
            var btnLogout = new Button()
            {
                Text = "Cerrar Sesión",
                Left = 360,
                Top = 15,
                Width = 120
            };
            btnLogout.Click += (_, __) => { BuildLoginUi(); };
            Controls.AddRange(new Control[] { lbl, btnLogout });
        }
    }
}
