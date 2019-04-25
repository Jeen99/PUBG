using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using CommonLibrary.CommonElements;
using System.Drawing;
using System.Windows.Media.Animation;
using System.Windows;

namespace BattleRoyalClient.Battle
{
	class Model3DGamer : Model3D
	{
		protected TypesWeapon lastCurrentWeapon;
		protected Model3D modelHands;

		public Model3DGamer(Model3DGroup models, IModelObject modelObject) : base(models, modelObject)
		{
			lastCurrentWeapon = TypesWeapon.Not;
			modelHands = new Model3D(models, new Hand(modelObject as Gamer));
		}
		public override void Remove()
		{
			base.Remove();
			modelHands.Remove();
			
		}
		public override void CreateImage()
		{
			base.CreateImage();
			modelHands.CreateImage();
		}
		public override void Update()
		{
			var pos = modelObject.Location;

			translateTransform.OffsetX = pos.X;
			translateTransform.OffsetY = pos.Y;

			modelHands.SetPosition(modelObject.Location3D);

			var gamer = (Gamer)modelObject;
			if (lastCurrentWeapon != gamer.CurrentWeapon)
			{
				lastCurrentWeapon = gamer.CurrentWeapon;
				modelHands.Remove();
				modelHands = new Model3D(models,new Hand(modelObject as Gamer));
				modelHands.CreateImage();
			}
			
			modelHands.Rotation(modelObject.Angle);
		}
	}
}
