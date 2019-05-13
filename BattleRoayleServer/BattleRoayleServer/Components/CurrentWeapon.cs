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
			Parent.Received_ChoiceWeapon -= Handler_ChoiceWeapon;
			Parent.Received_MakeShot -= Handler_MakeShot;
			Parent.Received_AddWeapon -= Handler_AddWeapon;
			inventory = null;
			currentWeapon = null;
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
				currentWeapon.Parent = Parent;
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

			Parent.Received_ChoiceWeapon += Handler_ChoiceWeapon;
			Parent.Received_MakeShot += Handler_MakeShot;
			Parent.Received_AddWeapon += Handler_AddWeapon;
		}
	}
}
