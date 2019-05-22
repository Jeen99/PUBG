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
		private List<Model3DVisualEffect> onlyVisuals = new List<Model3DVisualEffect>();
		private Model3DGroup group;

		public VisualConteyner(Model3DGroup group)
		{
			this.group = group;
		}
		//если возвращает true значит произошло добавление объекта
		public void AddOrUpdate(IModelObject modelObject, ulong ID)
		{

			if (!visuals.Keys.Contains(ID))
			{
				CreateModel3d(modelObject, ID);
				visuals[ID].CreateImage();
				
			}
			else
				UpdateVisual(ID);
		}

		public void AddOnlyVisual(IModelObject model)
		{
			var itemOnlyVisual = new Model3DVisualEffect(this.group, model, this);
			onlyVisuals.Add(itemOnlyVisual);
			itemOnlyVisual.CreateImage();
		}
		public void RemoveOnlyVisual(Model3DVisualEffect model)
		{
			onlyVisuals.Remove(model);
			model.Remove();
		}

		public void CreateModel3d(IModelObject model, ulong ID)
		{
			if (model is Gamer)
				visuals[ID] = new Model3DGamer(this.group, model);
			else visuals[ID] = new Model3D(this.group, model);
		}

		public void UpdateVisual(ulong ID)
		{
			visuals[ID].Update();
		}

		public bool DeleteModel3d(ulong ID)
		{
			Model3D model;
			var state = visuals.TryRemove(ID, out model);
			if (state)
				model.Remove();
			
			return state;
		}
	}
}
