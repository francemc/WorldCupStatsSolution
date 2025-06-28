using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WorldCupStats.Data.Models;

public partial class PlayerControl : UserControl
{
    public Player Player { get; }
    public bool IsFavorite { get; private set; }
    public bool IsSelected { get; private set; }

    private Label lblStar;
    private ContextMenuStrip contextMenu;

    public event EventHandler FavoriteToggled;
    public event EventHandler SelectedChanged;

    public PlayerControl(Player player, bool isFavorite = false)
    {
        Player = player;
        IsFavorite = isFavorite;
       
        InitializeComponents();
      
    
        SetupContextMenu();
        RegisterEvents();
        UpdateAppearance();
    }

    private void InitializeComponents()
    {
        this.Width = 355;
        this.Height = 50;
        this.BackColor = Color.White;
        this.Cursor = Cursors.Hand;
        this.AllowDrop = true;

        lblStar = new Label
        {
            Text = "★",
            ForeColor = Color.Gold,
            Location = new Point(5, 15),
            Visible = IsFavorite,
            AutoSize = true
        };

        var lblName = new Label
        {
            Text = Player.Name,
            Location = new Point(25, 5),
            AutoSize = true,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Tag = this
        };

        var lblDetails = new Label
        {
            Text = $"#{Player.ShirtNumber} | {Player.Position}" + (Player.Captain ? " (Captain)" : ""),
            Location = new Point(25, 25),
            AutoSize = true,
            Font = new Font("Segoe UI", 8),
            Tag = this
        };
        var pictureBox = new PictureBox
        {
            Size = new Size(40, 40),
            Location = new Point(305, 5),
            SizeMode = PictureBoxSizeMode.StretchImage,
            BorderStyle = BorderStyle.FixedSingle
        };

        pictureBox.Image = LoadPlayerImage(Player.Name);

        this.Controls.Add(pictureBox);

        this.Controls.Add(lblStar);
        this.Controls.Add(lblName);
        this.Controls.Add(lblDetails);

        // Propagate clicks from labels to this control's click
        lblName.Click += PlayerControl_Click;
        lblDetails.Click += PlayerControl_Click;
        lblStar.Click += PlayerControl_Click;

        this.Click += PlayerControl_Click;
    }

    private void SetupContextMenu()
    {
        contextMenu = new ContextMenuStrip();
        var toggleFavoriteItem = new ToolStripMenuItem(IsFavorite ? "Remove from Favorites" : "Add to Favorites");
        toggleFavoriteItem.Click += (s, e) => ToggleFavorite();
        contextMenu.Items.Add(toggleFavoriteItem);
        this.ContextMenuStrip = contextMenu;
    }

    private void RegisterEvents()
    {
        this.MouseDown += PlayerControl_MouseDown;
        this.MouseEnter += (s, e) => this.BackColor = Color.LightGray;
        this.MouseLeave += (s, e) => UpdateAppearance();

        foreach (Control child in this.Controls)
        {
            child.MouseDown += PlayerControl_MouseDown;
        }
    }

    private void PlayerControl_Click(object sender, EventArgs e)
    {
        ToggleSelection();
    }

    private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            var parent = this.Parent;
            if (parent == null) return;

            // Find all selected controls in the parent panel
            var allControls = parent.Controls.OfType<PlayerControl>().ToList();
            var selected = allControls.Where(pc => pc.IsSelected).ToList();

            // If this control is selected among others, drag all selected
            if (selected.Count > 1 && selected.Contains(this))
            {
                // Create a new list to avoid modification issues
                var selectedList = new List<PlayerControl>(selected);
                this.DoDragDrop(selectedList, DragDropEffects.Move);
            }
            else
            {
                // If only this control is selected or none are selected, drag just this one
                this.DoDragDrop(this, DragDropEffects.Move);
            }
        }
    }

    public void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
        UpdateAppearance();
        FavoriteToggled?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleSelection()
    {
        IsSelected = !IsSelected;
        UpdateAppearance();
        SelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Deselect()
    {
        IsSelected = false;
        UpdateAppearance();
    }

    public void SetFavorite(bool favorite)
    {
        if (IsFavorite != favorite)
        {
            IsFavorite = favorite;
            UpdateAppearance();
            FavoriteToggled?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateAppearance()
    {
        lblStar.Visible = IsFavorite;

        if (IsSelected)
        {
            this.BackColor = Color.LightBlue;
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        else
        {
            this.BackColor = IsFavorite ? Color.LightSlateGray
                : Color.White;
            this.BorderStyle = BorderStyle.None;
        }

        if (contextMenu.Items.Count > 0)
        {
            contextMenu.Items[0].Text = IsFavorite ? "Remove from Favorites" : "Add to Favorites";
        }
    }

    private Image LoadPlayerImage(string playerName)
    {
        string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images");
        string imageFile = Path.Combine(imageFolder, $"{playerName}.png");

        if (File.Exists(imageFile))
        {
            return Image.FromFile(imageFile);
        }

        string defaultImage = Path.Combine(imageFolder, "default.png");
        return File.Exists(defaultImage) ? Image.FromFile(defaultImage) : null;
    }

}
