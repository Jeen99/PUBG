using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BattleRoyalClient.Battle
{
	class VisualConteyner
	{
		private ConcurrentDictionary<ulong, Model3D> visuals = new ConcurrentDictionary<ulong, Model3D>();
		private Model3DGroup group;

		public VisualConteyner(Model3DGroup group)
		{
			this.group = group;
		}

		public void AddOrUpdate(IModelObject modelObject, ulong ID)
		{
			//var ID = modelObject.ID;

			if (!visuals.Keys.Contains(ID))
				CreateModel3d(modelObject, ID);
			else
				UpdateVisual(ID);
		}

		public void CreateModel3d(IModelObject model, ulong ID)
		{
			visuals[ID] = new Model3D(this.group, model);
		}

		public void UpdateVisual(ulong ID)
		{
			visuals[ID].UpdatePosition();
		}

		public bool DeleteModel3d(ulong ID)
		{
			Model3D model;
			return visuals.TryRemove(ID, out model);
		}
	}
}
