using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Model3D
	{
		protected SizeF lastSize;
		protected TranslateTransform3D translateTransform; // Матрица перемещения
		protected RotateTransform3D rotationTransform; // Матрица вращения
		protected MeshGeometry3D mesh;
		protected Model3DGroup models;
		protected GeometryModel3D geometryModel;

		protected IModelObject modelObject;

		protected static readonly string pathResources = "Resources/";

		public Model3D(Model3DGroup models, IModelObject modelObject)
		{
			this.modelObject = modelObject;
			this.models = models;
			lastSize = modelObject.Size;
		}
		//при смене оружия пересоздаем изображение
		public void CreateImage()
		{
			string pathImage = pathResources + modelObject.TextureName + ".png";
			mesh = new MeshGeometry3D();

			CreateSize(mesh);
			mesh.TriangleIndices = new Int32Collection(new List<int> { 0, 1, 2, 0, 2, 3 });
			mesh.TextureCoordinates = new PointCollection
			{
				// Устанавливаем текстурные координаты чтоб потом могли натянуть текстуру
				new Point(0, 1),
				new Point(1, 1),
				new Point(1, 0),
				new Point(0, 0)
			};

			ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(pathImage, UriKind.Relative)));
			Material material = new DiffuseMaterial(brush);
			geometryModel = new GeometryModel3D(mesh, material);
			models.Children.Add(geometryModel);

			
			
			var location = modelObject.Location3D;
			translateTransform = new TranslateTransform3D(new Vector3D(location.X, location.Y, location.Z));
			var Axis = new Vector3D(lastSize.Height / 2, lastSize.Height / 2, 0);
			rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(Axis, modelObject.Angle), Axis.X, Axis.Y, Axis.Z);

			Transform3DGroup tgroup = new Transform3DGroup();
			tgroup.Children.Add(translateTransform);
			tgroup.Children.Add(rotationTransform);
			geometryModel.Transform = tgroup;

			//сортировка по оси z		
			if (models.Children.Count >= 2)
			{
				SortComposition();
			}

		}

		protected void CreateSize(MeshGeometry3D mesh)
		{
			mesh.Positions = new Point3DCollection(new List<Point3D>
			{
				new Point3D(-lastSize.Width/2f, -lastSize.Height/2f, 0),
				new Point3D(lastSize.Width/2f, -lastSize.Height/2f, 0),
				new Point3D(lastSize.Width/2f, lastSize.Height/2f, 0),
				new Point3D(-lastSize.Width/2f, lastSize.Height/2f, 0)
			});
		}

		protected int ComparisonForSort(System.Windows.Media.Media3D.Model3D x,
			System.Windows.Media.Media3D.Model3D y)
		{
			if (x.Bounds.Z < y.Bounds.Z) return -1;
			else if (x.Bounds.Z > y.Bounds.Z) return 1;
			return 0;
		}

		protected void SortComposition()
		{
			//получаем список объктов композиции
			List<System.Windows.Media.Media3D.Model3D> collection = models.Children.ToList();
			models.Children.Clear();
			collection.Sort(ComparisonForSort);
			foreach (var item in collection)
			{
				 models.Children.Add(item);
			}
		}

		// Утсанавливает позицию объекта
		public virtual void SetPosition(Vector3D v3)
		{
			translateTransform.OffsetX = v3.X;
			translateTransform.OffsetY = v3.Y;
			translateTransform.OffsetZ = v3.Z;
		}

		public virtual void UpdatePosition()
		{
			//translateTransform.Transform(modelObject.Location);
			var pos = modelObject.Location3D;

			translateTransform.OffsetX = pos.X;
			translateTransform.OffsetY = pos.Y;
			translateTransform.OffsetZ = pos.Z;
		}

		public virtual void Update()
		{
			var pos = modelObject.Location3D;

			translateTransform.OffsetX = pos.X;
			translateTransform.OffsetY = pos.Y;
			translateTransform.OffsetZ = pos.Z;

			if (modelObject is DeathZone)
			{
				if (lastSize.Width != modelObject.Size.Width ||
					lastSize.Height != modelObject.Size.Height)
				{
					lastSize = modelObject.Size;
					CreateSize(mesh);
				}
			}
			
			
		}

		public Vector3D GetPosition()
		{
			return new Vector3D(translateTransform.OffsetX, translateTransform.OffsetY, translateTransform.OffsetZ);
		}

		// Поворачивает объект
		public virtual void Rotation(Vector3D axis, double angle, double centerX = 0.5, double centerY = 0.5, double centerZ = 0.5)
		{
			rotationTransform.CenterX = translateTransform.OffsetX;
			rotationTransform.CenterY = translateTransform.OffsetY;
			rotationTransform.CenterZ = translateTransform.OffsetZ;

			rotationTransform.Rotation = new AxisAngleRotation3D(axis, angle);
		}
		
		public virtual void Remove()
		{
			models.Children.Remove(geometryModel);
			
		}
	}
}
