using System;

namespace HoloRobot.Robot.Program
{
    public interface IRobotProgram
    {
        public void SendProgram();

        public void GetProgram();

        public void GetStatusProgram();
    }
}