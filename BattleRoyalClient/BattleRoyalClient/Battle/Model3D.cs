﻿using System;
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

namespace BattleRoyalClient
{
	class Model3D
	{
		public Vector3D Position { get; set; } // Позиция квадрата
		public SizeF Size { get; set; } // Размер квадрата
		private TranslateTransform3D translateTransform; // Матрица перемещения
		private RotateTransform3D rotationTransform; // Матрица вращения
		private Model3DGroup models;
		private GeometryModel3D geometryModel;

		private IModelObject modelObject;

		private static readonly string pathResources = "Resources/";

		public Model3D(Model3DGroup models, IModelObject modelObject)
		{
			this.modelObject = modelObject;
			this.models = models;

			double x = modelObject.Location3D.X;
			double y = modelObject.Location3D.Y;
			double z = modelObject.Location3D.Z;

			this.Position = new Vector3D(x, y, z);

			double angle = modelObject.Angle;

			float axis_x = 0;
			float axis_y = 0;
			float axis_z = 1;

			this.Size = modelObject.Size;

			string pathImage = pathResources + modelObject.TextureName + ".png";
			//string path = gameObject.Type.ToString();

			MeshGeometry3D mesh = new MeshGeometry3D();
			// Проставляем вершины квадрату
			mesh.Positions = new Point3DCollection(new List<Point3D>
			{
				new Point3D(-Size.Width/2, -Size.Height/2, 0),
				new Point3D(Size.Width/2, -Size.Height/2, 0),
				new Point3D(Size.Width/2, Size.Height/2, 0),
				new Point3D(-Size.Width/2, Size.Height/2, 0)
			});
			// Указываем индексы для квадрата
			mesh.TriangleIndices = new Int32Collection(new List<int> { 0, 1, 2, 0, 2, 3 });
			mesh.TextureCoordinates = new PointCollection
			{
				// Устанавливаем текстурные координаты чтоб потом могли натянуть текстуру
				new Point(0, 1),
				new Point(1, 1),
				new Point(1, 0),
				new Point(0, 0)
			};
			// Натягиваем текстуру
			ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(pathImage, UriKind.Relative)));
			Material material = new DiffuseMaterial(brush);

			geometryModel = new GeometryModel3D(mesh, material);
			models.Children.Add(geometryModel);

			translateTransform = new TranslateTransform3D(Position.X, Position.Y, Position.Z);
			rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(axis_x, axis_y, axis_z), angle), 0.5, 0.5, 0.5);

			Transform3DGroup tgroup = new Transform3DGroup();
			tgroup.Children.Add(translateTransform);
			tgroup.Children.Add(rotationTransform);
			geometryModel.Transform = tgroup;
		}
		// Утсанавливает позицию объекта
		public void SetPosition(Vector3D v3)
		{
			translateTransform.OffsetX = v3.X;
			translateTransform.OffsetY = v3.Y;
			translateTransform.OffsetZ = v3.Z;
		}

		public void UpdatePosition()
		{
			//translateTransform.Transform(modelObject.Location);
			var pos = modelObject.Location3D;

			translateTransform.OffsetX = pos.X;
			translateTransform.OffsetY = pos.Y;
			translateTransform.OffsetZ = pos.Z;
		}

		public Vector3D GetPosition()
		{
			return new Vector3D(translateTransform.OffsetX, translateTransform.OffsetY, translateTransform.OffsetZ);
		}

		// Поворачивает объект
		public void Rotation(Vector3D axis, double angle, double centerX = 0.5, double centerY = 0.5, double centerZ = 0.5)
		{
			rotationTransform.CenterX = translateTransform.OffsetX;
			rotationTransform.CenterY = translateTransform.OffsetY;
			rotationTransform.CenterZ = translateTransform.OffsetZ;

			rotationTransform.Rotation = new AxisAngleRotation3D(axis, angle);
		}
		public SizeF GetSize()
		{
			return Size;
		}

		public void Remove()
		{
			models.Children.Remove(geometryModel);
		}
	}
}