"""
ENS160.py MicroPython driver for the ENS160 Digital Metal-Oxide Multi-Gas Sensor manufactured by ScioSense - https://www.sciosense.com/products/environmental-sensors/ens160-digital-multi-gas-sensor/
Author Tim Hanewich, github.com/TimHanewich
Find updates to this code: https://github.com/TimHanewich/MicroPython-Collection/blob/master/ENS160/ENS160.py

MIT License
Copyright 2023 Tim Hanewich
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
"""

import machine

class ENS160:
    """
    Lightweight class for communicating with an ENS160 air quality sensor via I2C.
    Follows the specifications defined in the official ENS160 data sheet: https://www.mouser.com/datasheet/2/1081/SC_001224_DS_1_ENS160_Datasheet_Rev_0_95-2258311.pdf
    Newer version of ENS160 data sheet: https://www.sciosense.com/wp-content/uploads/documents/SC-001224-DS-9-ENS160-Datasheet.pdf
    """
    
    def __init__(self, i2c:machine.I2C, address:int = 0x53):
        """
        Creates a new instance of the ENS160 class
        :param i2c: Setup machine.I2C interface
        :param address: The I2C address of the ENS160 slave device
        """
        self.address = address
        self.i2c = i2c
    
    def __str__(self) -> str:
        ToReturn = "Operating Mode: " + self.operating_mode["text"]
        ToReturn = ToReturn + "\n" + "AQI: " + str(self.AQI)
        ToReturn = ToReturn + "\n" + "TVOC: " + str(self.TVOC)
        ToReturn = ToReturn + "\n" + "ECO2: " + str(self.ECO2)
        ToReturn = ToReturn + "\n" + "Status: " + str(self.status)
        return ToReturn

    @property
    def operating_mode(self) -> dict:
        """
        Reads the operating mode that the ENS160 is currently in.
        0 = Deep Sleep Mode (low power standby)
        1 = Idle mode (low-power)
        2 = Standard Gas Sensing Mode
        """
        op_mode:int = self.i2c.readfrom_mem(self.address, 0x10, 1)[0]

        if op_mode == 0:
            return {"value": 0, "text": "deep sleep"}
        elif op_mode == 1:
            return {"value": 1, "text": "idle"}
        elif op_mode == 2:
            return {"value": 2, "text": "standard gas sensing"}
        else:
            return {"value": op_mode, "text": "(unknown)"}
    
    @operating_mode.setter
    def operating_mode(self, value):
        """
        Sets the ENS160's operating mode.
        0 = Deep Sleep Mode (low power standby)
        1 = Idle mode (low-power)
        2 = Standard Gas Sensing Mode
        """
        if value not in [0, 1, 2, 0xF0]: # 0xF0 is supposedly a reset command that I've only seen in some datasheets. Leaving it here as an acceptable option, but that isn't advertised in this functions description.
            raise Exception("Operating value you're setting must be 0, 1, or 2")
        self.i2c.writeto_mem(self.address, 0x10, bytes([value]))
        
    @property
    def ECO2(self) -> int:
        """Reads the calculated equivalent CO2-concentration in PPM, based on the detected VOCs and hydrogen"""
        bs = self.i2c.readfrom_mem(self.address, 0x24, 2)
        return self._translate_pair(bs[1], bs[0])
        
    @property
    def TVOC(self) -> int:
        """Reads the calculated Total Volatile Organic Compounds (TVOC) concentration in ppb"""
        bs = self.i2c.readfrom_mem(self.address, 0x22, 2)
        return self._translate_pair(bs[1], bs[0])
    
    @property
    def AQI(self) -> dict:
        """
        Reads the calculated Air Quality Index (AQI) according to the UBA
        1 = Excellent
        2 = Good
        3 = Moderate
        4 = Poor
        5 = Unhealthy
        """
        val:int = self.i2c.readfrom_mem(self.address, 0x21, 1)[0]

        if val == 1:
            return {"value": val, "text": "excellent"}
        elif val == 2:
            return {"value": val, "text": "good"}
        elif val == 3:
            return {"value": val, "text": "moderate"}
        elif val == 4:
            return {"value": val, "text": "poor"}
        elif val == 5:
            return {"value": val, "text": "unhealthy"}
        else:
            return {"value": val, "text": "(unknown)"}

    @property
    def status(self) -> dict:
        """Reads the DATA_STATUS register and translates this single byte into each field's value."""
        statusb:int = self.i2c.readfrom_mem(self.address, 0x20, 1)[0]
        statuss:str = self._byte_to_binary(statusb)
        
        ToReturn:dict = {}

        # STATAS
        if statuss[0] == "1":
            ToReturn["STATAS"] = True
        else:
            ToReturn["STATAS"] = False

        # STATER (error)
        if statuss[1] == "1":
            ToReturn["STATER"] = True
        else:
            ToReturn["STATER"] = False
        
        # validity flag
        vf = statuss[4] + statuss[5]
        if vf == "00":
            ToReturn["VALIDITY FLAG"] = 0
        elif vf == "01":
            ToReturn["VALIDITY FLAG"] = 1
        elif vf == "10":
            ToReturn["VALIDITY FLAG"] = 2
        elif vf == "11":
            ToReturn["VALIDITY FLAG"] = 3

        # New data
        if statuss[6] == "1":
            ToReturn["NEWDAT"] = True
        else:
            ToReturn["NEWDAT"] = False
        
        # New GPR (General Purpose Read registers)
        if statuss[7] == "1":
            ToReturn["NEWGPR"] = True
        else:
            ToReturn["NEWGPR"] = True

        return ToReturn

    @property
    def error(self) -> bool:
        return self.status["STATER"]
        
    @property
    def new_data(self) -> bool:
        """Indicates if new sensor data is available to read."""
        return self.status["NEWDAT"]
        
    @property
    def signal_rating(self) -> dict:
        """
        Also referred to as 'Validity Flag', assesses the current operational mode and reliability of output signals.
        
        Values can be interpreted as:
        0: Normal operation
        1: Warm-Up phase
        2: Initial Start-Up phase
        3: Invalid output
        """

        SR:int = self.status["VALIDITY FLAG"]
        if SR == 0:
            return {"value": 0, "text": "Normal operation"}
        elif SR == 1:
            return {"value": 1, "text": "Warm-Up phase"}
        elif SR == 2:
            return {"value": 2, "text": "Initial Start-Up phase"}
        elif SR == 3:
            return {"value": 3, "text": "Invalid output"}

    @property
    def temperature(self) -> float:
        """Reports the temperature used in calculations, in Celsius."""
        DATA_T:bytes = self.i2c.readfrom_mem(self.address, 0x30, 2) # read raw bytes
        DATA_T_i:int = int.from_bytes(DATA_T, "little") # convert bytes to int
        ToReturnF:float = DATA_T_i / 64 # it is stored *64 (weird, idk why), so undo it.
        ToReturnF = ToReturnF - 273.15
        return ToReturnF

    @temperature.setter
    def temperature(self, temperature_c:float) -> None:
        """Sets the temperature, in Celsius, that the ENS160 uses in its calculations."""
        kelvin:float = temperature_c + 273.15 # the ENS160 stores as Kelvin
        to_write:float = kelvin * 64 # for whatever reason, the ENS160 stores the temperature, in kelvin, multiplied by 64. Weird, but okay.
        asbs:bytes = int(to_write).to_bytes(2, "little")
        self.i2c.writeto_mem(self.address, 0x13, asbs) # write to TEMP_IN on address 0x13

    @property
    def humidity(self) -> float:
        """Reports the relative humidity used in calculations."""
        DATA_RH:bytes = self.i2c.readfrom_mem(self.address, 0x32, 2)
        DATA_RH_i:int = int.from_bytes(DATA_RH, "little")
        ToReturnF:float = DATA_RH_i / 512
        ToReturnF = ToReturnF / 100 # return as between 0.0 and 1.0 (a percentage)
        return ToReturnF

    @humidity.setter
    def humidity(self, relative_humidity:float) -> None:
        """Sets the relative humidity (between 0.0 and 1.0) that will be used in calculations."""
        if relative_humidity < 0.0 or relative_humidity > 1.0:
            raise Exception("Please provide your relative humidity between 0.0 and 1.0 (a percentage).")
        rhi:int = int(round(relative_humidity * 100, 0)) # the RH will be provided between 0.0 and 1.0. So scale it up to two digits.
        rhi = rhi * 512 # the ENS160 stores it as it is multiplied by 512
        asbs:bytes = rhi.to_bytes(2, "little")
        self.i2c.writeto_mem(self.address, 0x15, asbs)



    def _translate_pair(self, high:int, low:int) -> int:
        """Converts a byte pair to a usable value. Borrowed from https://github.com/m-rtijn/mpu6050/blob/0626053a5e1182f4951b78b8326691a9223a5f7d/mpu6050/mpu6050.py#L76C39-L76C39."""
        value = (high << 8) + low
        if value >= 0x8000:
            value = -((65535 - value) + 1)
        return value   
    
    def _byte_to_binary(self, byte:int) -> str:
        """Converts a byte into a 8-character string, each character representing a single bit, being 0 or 1."""
        if byte < 0 or byte > 255:
            raise Exception("Value of '" + str(byte) + "' is not a byte. Unable to convert.")
        binary:str = ""
        for x in range(8):
            binary = str(byte & 1) + binary
            byte >>= 1
        return binary