using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Common;
using Math = System.Math;
using System.Drawing;

namespace BattleRoyalServer.Tests
{
	[TestClass()]
	public class VectorMethodTests
	{
		[TestMethod()]
		public void RotateVectorTest_1()
		{
			//Arrange (подготовка)
			float angle1 = 45F;
			float distance1 = (float) Math.Sqrt(1+1);
			float x1 = 1F;
			float y1 = 1F;
			//Act (выполнение)
			Vec2 vector1 = VectorMethod.RotateVector(angle1, distance1);
			//Assert (проверка)
			Assert.AreEqual(x1, vector1.X);
			Assert.AreEqual(y1, vector1.Y);
		}

		[TestMethod()]
		public void RotateVectorTest_2()
		{
			//Arrange (подготовка)
			float angle2 = 0F;
			float distance2 = 20F;
			float x2 = 20F;
			float y2 = 0F;
			//Act (выполнение)
			Vec2 vector2 = VectorMethod.RotateVector(angle2, distance2);
			//Assert (проверка)
			Assert.AreEqual(x2, vector2.X);
			Assert.AreEqual(y2, vector2.Y);
		}

		[TestMethod()]
		public void RotateVectorTest_3()
		{
			//Arrange (подготовка)
			float angle3 = 90F;
			float distance3 = 20F;
			float x3 = 0F;
			float y3 = 20F;
			//Act (выполнение)
			Vec2 vector3 = VectorMethod.RotateVector(angle3, distance3);
			//Assert (проверка)
			Assert.AreEqual(x3, vector3.X);
			Assert.AreEqual(y3, vector3.Y);
		}


		[TestMethod()]
		public void RotateVectorTest_4()
		{
			//Arrange (подготовка)
			float angle4 = 270F;
			float distance4 = 20F;
			float x4 = 0F;
			float y4 = -20F;
			//Act (выполнение)
			Vec2 vector4 = VectorMethod.RotateVector(angle4, distance4);
			//Assert (проверка)
			Assert.AreEqual(x4, vector4.X);
			Assert.AreEqual(y4, vector4.Y);
		}

		[TestMethod()]
		public void DefineDistanceTest1()
		{
			//Arrange (подготовка)
			Vec2 v1 = new Vec2(0, 0);
			Vec2 v2 = new Vec2(0, 10);
			//Act (выполнение)
			float resultDistance = VectorMethod.DefineDistance(v1, v2);
			//Assert (проверка)
			Assert.AreEqual(10, resultDistance);
		}

		[TestMethod()]
		public void DefineDistanceTest2()
		{
			//Arrange (подготовка)
			Vec2 v1 = new Vec2(6, 2);
			Vec2 v2 = new Vec2(30, 9);
			//Act (выполнение)
			float resultDistance = VectorMethod.DefineDistance(v1, v2);
			//Assert (проверка)
			Assert.AreEqual(25, resultDistance);
		}

		[TestMethod()]
		public void DefineAngleTest1()
		{
			//Arrange (подготовка)
			Vec2 start = new Vec2();
			Vec2 end = new Vec2(0, 10);
			//Act (выполнение)
			float angle = VectorMethod.DefineAngle(start, end);
			//Assert (проверка)
			Assert.AreEqual(90, angle);
		}


		[TestMethod()]
		public void DefineAngleTest2()
		{
			//Arrange (подготовка)
			Vec2 start = new Vec2();
			Vec2 end = new Vec2(10, 0);
			//Act (выполнение)
			float angle = VectorMethod.DefineAngle(start, end);
			//Assert (проверка)
			Assert.AreEqual(0, angle);
		}

		[TestMethod()]
		public void DefineAngleTest3()
		{
			//Arrange (подготовка)
			Vec2 start = new Vec2();
			Vec2 end = new Vec2(10, 10);
			//Act (выполнение)
			float angle = VectorMethod.DefineAngle(start, end);
			//Assert (проверка)
			Assert.AreEqual(45, angle);
		}

		[TestMethod()]
		public void CreateRandPositionTest()
		{
			//Arrange (подготовка)
			int size = 1;
			//Act (выполнение)
			PointF point = VectorMethod.CreateRandPosition(size);

			float X = point.X;
			float Y = point.Y;
			//Assert (проверка)
			if (X != 0 && X != 1)
				Assert.Fail();

			if (Y != 0 && Y != 1)
				Assert.Fail();
		}
	}
}