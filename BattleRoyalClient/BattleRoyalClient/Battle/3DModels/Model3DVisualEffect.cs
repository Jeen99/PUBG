using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Timers;

namespace BattleRoyalClient.Battle
{
	class Model3DVisualEffect : Model3D
	{

		private VisualConteyner forSelfDeleted;

		private Timer timerVisisble = new Timer()
		{
			AutoReset = false,
			Interval = 250
		};

		public Model3DVisualEffect(Model3DGroup models, IModelObject modelObject, VisualConteyner conteyner) : base(models, modelObject)
		{
			timerVisisble.Elapsed += TimerVisisble_Elapsed;
			forSelfDeleted = conteyner;
		}

		private void TimerVisisble_Elapsed(object sender, ElapsedEventArgs e)
		{
			forSelfDeleted.RemoveOnlyVisual(this);
		}

		public override void CreateImage()
		{
			base.CreateImage();
			Rotation(-modelObject.Angle);
			timerVisisble.Start();
		}

	}
}
