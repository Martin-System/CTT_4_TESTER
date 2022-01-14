using System;
using Automation.BDaq;

public class P4Relay
{
	byte maskUsbCharger = 0x01;
	byte maskWpcCharger = 0x02;
	byte maskDiode = 0x04;
	byte maskBatteryResistor = 0x08;

	InstantDoCtrl doCtrl;
	public P4Relay(InstantDoCtrl doCtrl,int deviceNumber)
	{
		this.doCtrl = doCtrl;
		this.doCtrl.SelectedDevice = new DeviceInformation(deviceNumber);
	}

	public ErrorCode init()
	{
		return this.doCtrl.Write(0, (byte)0x00);

	}

	public ErrorCode setStandby()
	{
		ErrorCode err;
		byte portData = 0x00;
		err = this.doCtrl.Write(0, portData);

		return err;
	}

	public ErrorCode setUsbCharging(bool state)
	{
		ErrorCode err;
		byte portData = 0x00;
		if (state)
		{
			portData = (byte)(maskBatteryResistor | maskUsbCharger | maskDiode);
		}
		else
		{
			portData = (byte)(maskBatteryResistor | maskDiode);
		}
		err = this.doCtrl.Write(0, portData);

		return err;
	}

	public ErrorCode setWpcCharging(bool state)
	{
		ErrorCode err;
		byte portData = 0x00;
		if (state)
		{
			portData = (byte)(maskBatteryResistor | maskWpcCharger | maskDiode);
		}
		else
		{
			portData = (byte)(maskBatteryResistor | maskDiode);
		}

		err = this.doCtrl.Write(0, portData);

		return err;
	}
}
