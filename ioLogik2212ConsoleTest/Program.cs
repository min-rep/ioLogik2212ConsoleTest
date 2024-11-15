//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MOXA_CSharp_MXIO;
//---------------------------------------------------------------------------
namespace MOXA_TEST
{

    class ioLogik
    {
       
        public const UInt16 Port = 502;						//Modbus TCP port
        public const UInt16 DO_SAFE_MODE_VALUE_OFF = 0;
        public const UInt16 DO_SAFE_MODE_VALUE_ON = 1;
        public const UInt16 DO_SAFE_MODE_VALUE_HOLD_LAST = 2;

        public const UInt16 DI_DIRECTION_DI_MODE = 0;
        public const UInt16 DI_DIRECTION_COUNT_MODE = 1;
        public const UInt16 DO_DIRECTION_DO_MODE = 0;
        public const UInt16 DO_DIRECTION_PULSE_MODE = 1;

        public const UInt16 TRIGGER_TYPE_LO_2_HI = 0;
        public const UInt16 TRIGGER_TYPE_HI_2_LO = 1;
        public const UInt16 TRIGGER_TYPE_BOTH = 2;
        //A-OPC Server response W5340 Device STATUS information data filed index
        public const int IP_INDEX = 0;
        public const int MAC_INDEX = 4;
        //===================================================================

        public static void CheckErr(int iRet, string szFunctionName)
        {
            string szErrMsg = "MXIO_OK";

            if (iRet != MXIO_CS.MXIO_OK)
            {

                switch (iRet)
                {
                    case MXIO_CS.ILLEGAL_FUNCTION:
                        szErrMsg = "ILLEGAL_FUNCTION";
                        break;
                    case MXIO_CS.ILLEGAL_DATA_ADDRESS:
                        szErrMsg = "ILLEGAL_DATA_ADDRESS";
                        break;
                    case MXIO_CS.ILLEGAL_DATA_VALUE:
                        szErrMsg = "ILLEGAL_DATA_VALUE";
                        break;
                    case MXIO_CS.SLAVE_DEVICE_FAILURE:
                        szErrMsg = "SLAVE_DEVICE_FAILURE";
                        break;
                    case MXIO_CS.SLAVE_DEVICE_BUSY:
                        szErrMsg = "SLAVE_DEVICE_BUSY";
                        break;
                    case MXIO_CS.EIO_TIME_OUT:
                        szErrMsg = "EIO_TIME_OUT";
                        break;
                    case MXIO_CS.EIO_INIT_SOCKETS_FAIL:
                        szErrMsg = "EIO_INIT_SOCKETS_FAIL";
                        break;
                    case MXIO_CS.EIO_CREATING_SOCKET_ERROR:
                        szErrMsg = "EIO_CREATING_SOCKET_ERROR";
                        break;
                    case MXIO_CS.EIO_RESPONSE_BAD:
                        szErrMsg = "EIO_RESPONSE_BAD";
                        break;
                    case MXIO_CS.EIO_SOCKET_DISCONNECT:
                        szErrMsg = "EIO_SOCKET_DISCONNECT";
                        break;
                    case MXIO_CS.PROTOCOL_TYPE_ERROR:
                        szErrMsg = "PROTOCOL_TYPE_ERROR";
                        break;
                    case MXIO_CS.SIO_OPEN_FAIL:
                        szErrMsg = "SIO_OPEN_FAIL";
                        break;
                    case MXIO_CS.SIO_TIME_OUT:
                        szErrMsg = "SIO_TIME_OUT";
                        break;
                    case MXIO_CS.SIO_CLOSE_FAIL:
                        szErrMsg = "SIO_CLOSE_FAIL";
                        break;
                    case MXIO_CS.SIO_PURGE_COMM_FAIL:
                        szErrMsg = "SIO_PURGE_COMM_FAIL";
                        break;
                    case MXIO_CS.SIO_FLUSH_FILE_BUFFERS_FAIL:
                        szErrMsg = "SIO_FLUSH_FILE_BUFFERS_FAIL";
                        break;
                    case MXIO_CS.SIO_GET_COMM_STATE_FAIL:
                        szErrMsg = "SIO_GET_COMM_STATE_FAIL";
                        break;
                    case MXIO_CS.SIO_SET_COMM_STATE_FAIL:
                        szErrMsg = "SIO_SET_COMM_STATE_FAIL";
                        break;
                    case MXIO_CS.SIO_SETUP_COMM_FAIL:
                        szErrMsg = "SIO_SETUP_COMM_FAIL";
                        break;
                    case MXIO_CS.SIO_SET_COMM_TIME_OUT_FAIL:
                        szErrMsg = "SIO_SET_COMM_TIME_OUT_FAIL";
                        break;
                    case MXIO_CS.SIO_CLEAR_COMM_FAIL:
                        szErrMsg = "SIO_CLEAR_COMM_FAIL";
                        break;
                    case MXIO_CS.SIO_RESPONSE_BAD:
                        szErrMsg = "SIO_RESPONSE_BAD";
                        break;
                    case MXIO_CS.SIO_TRANSMISSION_MODE_ERROR:
                        szErrMsg = "SIO_TRANSMISSION_MODE_ERROR";
                        break;
                    case MXIO_CS.PRODUCT_NOT_SUPPORT:
                        szErrMsg = "PRODUCT_NOT_SUPPORT";
                        break;
                    case MXIO_CS.HANDLE_ERROR:
                        szErrMsg = "HANDLE_ERROR";
                        break;
                    case MXIO_CS.SLOT_OUT_OF_RANGE:
                        szErrMsg = "SLOT_OUT_OF_RANGE";
                        break;
                    case MXIO_CS.CHANNEL_OUT_OF_RANGE:
                        szErrMsg = "CHANNEL_OUT_OF_RANGE";
                        break;
                    case MXIO_CS.COIL_TYPE_ERROR:
                        szErrMsg = "COIL_TYPE_ERROR";
                        break;
                    case MXIO_CS.REGISTER_TYPE_ERROR:
                        szErrMsg = "REGISTER_TYPE_ERROR";
                        break;
                    case MXIO_CS.FUNCTION_NOT_SUPPORT:
                        szErrMsg = "FUNCTION_NOT_SUPPORT";
                        break;
                    case MXIO_CS.OUTPUT_VALUE_OUT_OF_RANGE:
                        szErrMsg = "OUTPUT_VALUE_OUT_OF_RANGE";
                        break;
                    case MXIO_CS.INPUT_VALUE_OUT_OF_RANGE:
                        szErrMsg = "INPUT_VALUE_OUT_OF_RANGE";
                        break;
                }

                Console.WriteLine("Function \"{0}\" execution Fail. Error Message : {1}\n", szFunctionName, szErrMsg);

                if (iRet == MXIO_CS.EIO_TIME_OUT || iRet == MXIO_CS.HANDLE_ERROR)
                {
                    //To terminates use of the socket
                    MXIO_CS.MXEIO_Exit();
                    Console.WriteLine("Press any key to close application\r\n");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
        }
        //----------------------------------------------------------------------------------
        static void Main(string[] args)
        {
            int ret;
            Int32[] hConnection = new Int32[1];
            string IPAddr = "192.168.127.254";
            UInt32 Timeout = 5000;
            UInt32 i;
            UInt16 wIndex;
            UInt32 uiGetInput = 0;

            {
                ret = MXIO_CS.MXIO_GetDllVersion();
                Console.WriteLine( "MXIO_GetDllVersion:{0}.{1}.{2}.{3}", (ret >> 12) & 0xF, (ret >> 8) & 0xF, (ret >> 4) & 0xF, (ret) & 0xF );
                Console.WriteLine("MXIO_GetDllVersion:{0}.{1}.{2}.{3} ret-> {4}", (ret >> 12) & 0xF, (ret >> 8) & 0xF, (ret >> 4) & 0xF, (ret) & 0xF, ret);

                ret = MXIO_CS.MXIO_GetDllBuildDate();
                //Console.WriteLine( "MXIO_GetDllBuildDate:{0:x}/{1:x}/{2:x}", (ret >> 16), (ret >> 8) & 0xFF, (ret) & 0xFF );
                Console.WriteLine("MXIO_GetDllBuildDate:{0:x}/{1:x}/{2:x} ret-> {3}", (ret >> 16), (ret >> 8) & 0xFF, (ret) & 0xFF, ret);
                //--------------------------------------------------------------------------
                ret = MXIO_CS.MXEIO_Init();
                Console.WriteLine("MXEIO_Init return {0}", ret);
                //--------------------------------------------------------------------------
                //total number of elemnets , Ex "192.168.127.254 2000" will return 2
                if (args.Length > 0)
                    IPAddr = args[0];

                if (args.Length > 1)
                {
                    //check when user enter non number
                    try
                    {
                        Timeout = UInt32.Parse(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("Only Enter Whole number for \"Timeout\" value.");
                    }
                }
                //--------------------------------------------------------------------------
                //Connect to ioLogik device
                Console.WriteLine("MXEIO_Connect IP={0}, Timeout={1}", IPAddr, Timeout);
                ret = MXIO_CS.MXEIO_Connect(System.Text.Encoding.UTF8.GetBytes(IPAddr), Port, Timeout, hConnection);
                CheckErr(ret, "MXEIO_Connect");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("MXEIO_Connect Success.");
                //--------------------------------------------------------------------------
                //Check Connection
                byte[] bytCheckStatus = new byte[1];
                ret = MXIO_CS.MXEIO_CheckConnection(hConnection[0], Timeout, bytCheckStatus);
                CheckErr(ret, "MXEIO_CheckConnection");
                if (ret == MXIO_CS.MXIO_OK)
                {
                    switch (bytCheckStatus[0])
                    {
                        case MXIO_CS.CHECK_CONNECTION_OK:
                            Console.WriteLine("MXEIO_CheckConnection: Check connection ok => {0}", bytCheckStatus[0]);
                            break;
                        case MXIO_CS.CHECK_CONNECTION_FAIL:
                            Console.WriteLine("MXEIO_CheckConnection: Check connection fail => {0}", bytCheckStatus[0]);
                            break;
                        case MXIO_CS.CHECK_CONNECTION_TIME_OUT:
                            Console.WriteLine("MXEIO_CheckConnection: Check connection time out => {0}", bytCheckStatus[0]);
                            break;
                        default:
                            Console.WriteLine("MXEIO_CheckConnection: Check connection status unknown => {0}", bytCheckStatus[0]);
                            break;
                    }
                }
                //--------------------------------------------------------------------------
                //Get firmware Version
                byte[] bytRevision = new byte[4];
                ret = MXIO_CS.MXIO_ReadFirmwareRevision(hConnection[0], bytRevision);
                CheckErr(ret, "MXIO_ReadFirmwareRevision");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("MXIO_ReadFirmwareRevision:V{0}.{1}, Release:{2}, build:{3}", bytRevision[0], bytRevision[1], bytRevision[2], bytRevision[3]);
                //--------------------------------------------------------------------------
                //Get firmware Release Date
                UInt16[] wGetFirmwareDate = new UInt16[2];
                ret = MXIO_CS.MXIO_ReadFirmwareDate(hConnection[0], wGetFirmwareDate);
                CheckErr(ret, "MXIO_ReadFirmwareDate");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("MXIO_ReadFirmwareDate:{0:x}/{1:x}/{2:x}", wGetFirmwareDate[1], (wGetFirmwareDate[0] >> 8) & 0xFF, (wGetFirmwareDate[0]) & 0xFF);
                //--------------------------------------------------------------------------
                //Get Module Type
                UInt16[] wModuleType = new UInt16[1];
                ret = MXIO_CS.MXIO_GetModuleType(hConnection[0], 0, wModuleType);
                CheckErr(ret, "MXIO_GetModuleType");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("MXIO_GetModuleType: Module Type = {0:x}", wModuleType[0]);
                //--------------------------------------------------------------------------
                byte bytCount = 0, bytStartChannel = 0;
                //Get DIO Direction Status
                UInt16[] wGetDOMode = new UInt16[4];        //Get DO Direction Mode
                UInt16[] wSetDO_DOMode = new UInt16[2];     //Set DO Direction DO Mode
                UInt16[] wSetDO_PulseMode = new UInt16[2];  //Set DO Direction Pulse Mode
                UInt16[] wGetDIMode = new UInt16[8];        //Get DI Direction Mode
                UInt16[] wSetDI_DIMode = new UInt16[4];     //Set DI Direction DI Mode
                UInt16[] wSetDI_CounterMode = new UInt16[4];//Set DI Direction Counter Mode
                UInt16 wDI_DI_MODE = 0;                     //DI Direction DI Mode Value
                UInt16 wDO_DO_MODE = 0;                     //DO Direction DO Mode Value

                Int32 dwShiftValue;
                //Set Ch{0}~ch{1} DO Direction DO Mode
                bytCount = 2;
                bytStartChannel = 6;
                for (i = 0; i < bytCount; i++)
                    wSetDO_DOMode[i] = DO_DIRECTION_DO_MODE;
                ret = MXIO_CS.DO2K_SetModes(hConnection[0], bytStartChannel, bytCount, wSetDO_DOMode);
                CheckErr(ret, "DO2K_SetModes");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("DO2K_SetModes Set Ch{0}~ch{1} DO Direction DO Mode success.", bytStartChannel, bytCount + bytStartChannel - 1);
                //Set Ch{0}~ch{1} DO Direction DO Mode
                bytCount = 2;
                bytStartChannel = 6;
                ret = MXIO_CS.DO2K_GetModes(hConnection[0], bytStartChannel, bytCount, wSetDO_DOMode);
                CheckErr(ret, "DO2K_GetModes");
                if (ret == MXIO_CS.MXIO_OK)
                {
                    for (i = 0; i < bytCount; i++)
                        Console.WriteLine("DO2K_GetModes return {0}, ch{1}={2}", ret, i + bytStartChannel, (wGetDOMode[i] == wDO_DO_MODE) ? "DO_MODE" : "PULSE_MODE");
                }
                //Set Ch{0}~ch{1} DO Direction DO Mode value = ON
                UInt32 dwSetDOValue = 0x0003;   //2개 켤때, 0x0002 2개 끌때
                //UInt32 dwSetDOValue = 0x0001; //1개 셋
                //ret = MXIO_CS.DO_Writes(hConnection[0], 0, bytStartChannel, bytCount, dwSetDOValue);
                ret = MXIO_CS.DO_Writes(hConnection[0], 0, bytStartChannel, bytCount, dwSetDOValue);
                CheckErr(ret, "DO_Writes");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("DO_Writes Set Ch{0}~ch{1} DO Direction DO Mode value = ON success.", bytStartChannel, bytCount + bytStartChannel - 1);
                //Get Ch{0}~ch{1} DO Direction DO Mode value
                bytCount = 2;
                bytStartChannel = 6;
                UInt32[] dwGetDOValue = new UInt32[1];
                ret = MXIO_CS.DO_Reads(hConnection[0], 0, bytStartChannel, bytCount, dwGetDOValue);
                CheckErr(ret, "DO_Reads");
                if (ret == MXIO_CS.MXIO_OK)
                {
                    Console.WriteLine("DO_Reads Get Ch{0}~ch{1} DO Direction DO Mode value success.", bytStartChannel, bytCount + bytStartChannel - 1);
                    for (i = 0, dwShiftValue = 0; i < bytCount; i++, dwShiftValue++)
                        Console.WriteLine("DO value: ch[{0}] = {1}", i + bytStartChannel, ((dwGetDOValue[0] & (1 << dwShiftValue)) == 0) ? "OFF" : "ON");
                }
                //--------------------------------------------------------------------------
                //End Application
                ret = MXIO_CS.MXEIO_Disconnect(hConnection[0]);
                CheckErr(ret, "MXEIO_Disconnect");
                if (ret == MXIO_CS.MXIO_OK)
                    Console.WriteLine("MXEIO_Disconnect return {0}", ret);
                //--------------------------------------------------------------------------
                MXIO_CS.MXEIO_Exit();
                Console.WriteLine("MXEIO_Exit, Press Enter To Exit.");
                Console.ReadLine();
            }
        }
    }
}
