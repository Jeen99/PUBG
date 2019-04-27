using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;

namespace BattleRoayleServer
{
	public class CurrentWeapon : Component
	{ 
		private Collector inventory;
		private Weapon currentWeapon;

		public Weapon GetCurrentWeapon { get => currentWeapon; }

		public CurrentWeapon(IGameObject parent) : base(parent)
		{
			currentWeapon = null;
		}

		public override IMessage State
		{
			get
			{
				if (currentWeapon != null)
				{
					return new CurrentWeaponState(currentWeapon.TypeWeapon);
				}
				else
				{
					return null;
				}
			}
		}

		public override void Dispose()
		{
			inventory = null;
			currentWeapon = null;
		}

		public override void UpdateComponent(IMessage msg)
		{

			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте CurrentWeapon");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesMessage.ChoiceWeapon:
					Handler_ChoiceWeapon(msg);
					break;
				case TypesMessage.MakeShot:
					Handler_MakeShot(msg);
					break;
				case TypesMessage.AddWeapon:
					Handler_AddWeapon(msg);
					break;
			}
		}

		private void Handler_AddWeapon(IMessage msg)
		{
			//если это первое подобранное оружие делаем его выбранным
			if (currentWeapon == null)
			{
				ChangeWeapon(msg.TypeWeapon);
			}
		}
		private void Handler_MakeShot(IMessage shot)
		{
			if (currentWeapon != null)
			{
				currentWeapon.Update(shot);
			}
		}

		private void Handler_ChoiceWeapon(IMessage msg)
		{		
			ChangeWeapon(msg.TypeWeapon);		
		}

		private void ChangeWeapon(TypesWeapon type)
		{
			var weapon = inventory.GetWeapon(type);
			if (weapon != null)
			{
				currentWeapon = weapon;
				currentWeapon.Holder = Parent;
				//отправляем сообщение об этом
				Parent?.Model?.AddOutgoingMessage(new ChangedCurrentWeapon(Parent.ID, type));
			}
		}

		public override void Setup()
		{
			this.inventory = Parent.Components?.GetComponent<Collector>();
			if (inventory == null)
			{
				Log.AddNewRecord("Ошибка создания компонента CurrentWeapon", "Не получена сслыка на компонент Collector");
				throw new Exception("Ошибка создания компонента CurrentWeapon");
			}
		}
	}
}
