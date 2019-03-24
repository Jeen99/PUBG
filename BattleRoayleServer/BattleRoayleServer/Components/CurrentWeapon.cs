using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class CurrentWeapon : Component
	{
		private Collector inventory;
		private Weapon currentWeapon;
		public CurrentWeapon(GameObject parent, Collector collector) : base(parent)
		{
			inventory = collector;
			currentWeapon = null;
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.ChoiceWeapon:
					Handler_ChoiceWeapon(msg as ChoiceWeapon);
					break;
				case TypesProgramMessage.MakeShot:
					Handler_MakeShot(msg as MakeShot);
					break;
				case TypesProgramMessage.AddWeapon:
					Handler_AddWeapon(msg as AddWeapon);
					break;
			}
		}

		private void Handler_AddWeapon(AddWeapon msg)
		{
			//если это первое подобранное оружие делаем его выбранным
			if (currentWeapon == null)
			{
				currentWeapon = inventory.GetWeapon(msg.TypeWeapon);
				Parent.Model.HappenedEvents.Enqueue(new ChangedCurrentWeapon(msg.TypeWeapon));
			}
		}
		private void Handler_MakeShot(MakeShot shot)
		{
			if (currentWeapon != null)
			{
				currentWeapon.SendMessage(shot);
				//обновляем объект раньше очереди, тк выстрел операция с наивысшим приорететом
				currentWeapon.Update();
			}
		}

		private void Handler_ChoiceWeapon(ChoiceWeapon msg)
		{
			if (msg.ChooseWeapon != currentWeapon.TypeWeapon)
			{
				currentWeapon = inventory.GetWeapon(msg.ChooseWeapon);
				currentWeapon.SetBodyHolder((SolidBody)Parent.GetComponent(typeof(SolidBody)));
				//отправляем сообщение об этом
				Parent.Model.HappenedEvents.Enqueue(new ChangedCurrentWeapon(msg.ChooseWeapon));
			}
		}
	}
}
