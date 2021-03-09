using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Newtonsoft.Json;
using Simulator.Converters;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A Box with a scalable non-textured rectangular shape.
    /// </summary>
    internal class Wall : Box, IScalable
    {
        /// <summary>
        /// Gets or sets the colour of the wall.
        /// </summary>
        [Browsable(false)]
        [Category("Object")]
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the displayed colour of the wall as a System.Drawing.Color. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Box")]
        [DisplayName("Colour")]
        public System.Drawing.Color DisplayColour
        {
            get { return System.Drawing.Color.FromArgb(Colour.A, Colour.R, Colour.G, Colour.B); }
            set { Colour = new Color(value.R, value.G, value.B, value.A); texture.SetData(new[] { Colour }); }
        }

        /// <summary>
        /// Gets or sets the displayed dimensions of the wall as a System.Drawing.SizeF. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Box")]
        [DisplayName("Dimensions")]
        [TypeConverter(typeof(System.Drawing.SizeFConverter))]
        public System.Drawing.SizeF DiplayDimensions
        {
            get { return VectorSizeConverter.VectorToSize(ScaleConverter.ScaleVector(Dimensions, Scale, 1, true, 2)); }
            set { Dimensions = ScaleConverter.InverseScaleVector(VectorSizeConverter.SizeToVector(value), Scale, 1); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override float DiplaySize { get => base.DiplaySize; set => base.DiplaySize = value; }

        [Browsable(false)]
        public bool MaintainAspectRatio { get; set; }

        /// <summary>
        /// Parameterless constructor for Wall.
        /// </summary>
        public Wall()
        {
        }

        /// <summary>
        /// Constructor for Wall.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="colour">Colour of the object's texture.</param>
        /// <param name="restitutionCoefficient">Coefficient of restitution of the object.</param>
        /// <param name="dimensions">The width and height of the wall.</param>
        public Wall(string name, Vector2 position, Color colour, float restitutionCoefficient, Vector2 dimensions) : base(name, position, "wall", restitutionCoefficient, dimensions)
        {
            Colour = colour;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            texture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Colour });
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, BoundingBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }
        }
    }
}