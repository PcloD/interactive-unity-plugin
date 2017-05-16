﻿using System;
using System.Collections.Generic;

namespace Xbox.Services.Beam
{
    /// <summary>
    /// Represents a Beam Interactivity button control. All controls are created and 
    /// configured using the Beam Lab.
    /// </summary>
    public class BeamButtonControl : BeamControl
    {
        /// <summary>
        /// Text displayed on this button control.
        /// </summary>
        public string ButtonText
        {
            get;
            private set;
        }

        /// <summary>
        /// Spark cost assigned to this button control.
        /// </summary>
        public uint Cost
        {
            get;
            private set;
        }

        /// <summary>
        /// Time remaining (in milliseconds) before this button can be triggered again.
        /// </summary>
        public int RemainingCooldown
        {
            get
            {
                Int64 now = (Int64)DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                int timeRemaining = (int)(cooldownExpirationTime - now);
                if (timeRemaining < 0)
                {
                    timeRemaining = 0;
                }
                return timeRemaining;
            }
        }

        /// <summary>
        /// Current progress of this button control.
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether the button has transitioned from up to down since the last call to DoWork().
        /// </summary>
        public bool ButtonDown
        {
            get
            {
                bool isDown = false;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        isDown = ButtonCountState.CountOfButtonDownEvents > 0;
                    }
                }
                return isDown;
            }
        }

        /// <summary>
        /// Whether the button is pressed.
        /// </summary>
        public bool ButtonPressed
        {
            get
            {
                bool isPressed = false;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        isPressed = ButtonCountState.CountOfButtonPressEvents > 0;
                    }
                }
                return isPressed;
            }
        }

        /// <summary>
        /// Whether the button has transitioned from down to up since the last call to DoWork().
        /// </summary>
        public bool ButtonUp
        {
            get
            {
                bool isUp = false;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        isUp = ButtonCountState.CountOfButtonUpEvents > 0;
                    }
                }
                return isUp;
            }
        }

        /// <summary>
        /// The number of button downs since the last call to DoWork().
        /// </summary>
        public uint CountOfButtonDowns
        {
            get
            {
                uint countOfButtonDownEvents = 0;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        countOfButtonDownEvents = ButtonCountState.CountOfButtonDownEvents;
                    }
                }
                return countOfButtonDownEvents;
            }
        }

        /// <summary>
        /// The number of button presses since the last call to DoWork().
        /// </summary>
        public uint CountOfButtonPresses
        {
            get
            {
                uint countOfButtonPressEvents = 0;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        countOfButtonPressEvents = ButtonCountState.CountOfButtonPressEvents;
                    }
                }
                return countOfButtonPressEvents;
            }
        }

        /// <summary>
        /// The number of button ups since the last call to DoWork().
        /// </summary>
        public uint CountOfButtonUps
        {
            get
            {
                uint countOfButtonPressEvents = 0;
                if (ControlID != null)
                {
                    InternalButtonCountState ButtonCountState;
                    if (BeamManager._buttonStates.TryGetValue(ControlID, out ButtonCountState))
                    {
                        countOfButtonPressEvents = ButtonCountState.CountOfButtonUpEvents;
                    }
                }
                return countOfButtonPressEvents;
            }
        }

        /// <summary>
        /// Function to update the progress value for this button control.
        /// </summary>
        /// <param name="progress">Value from 0.0 to 1.0.</param>
        public void SetProgress(float progress)
        {
            BeamManager.SingletonInstance.SendSetControlProgress(ControlID, progress);
        }

        /// <summary>
        /// Whether a given Beam user triggered a button down since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public bool GetButtonDown(uint beamID)
        {
            return BeamManager.SingletonInstance.GetButtonDown(ControlID, beamID);
        }

        /// <summary>
        /// Whether a given Beam user triggered a button press since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public bool GetButtonPressed(uint beamID)
        {
            return BeamManager.SingletonInstance.GetButtonPressed(ControlID, beamID);
        }

        /// <summary>
        /// Whether a given Beam user triggered a button up since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public bool GetButtonUp(uint beamID)
        {
            return BeamManager.SingletonInstance.GetButtonUp(ControlID, beamID);
        }

        /// <summary>
        /// The number of button downs from a given Beam user since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public uint GetCountOfButtonDowns(uint beamID)
        {
            return BeamManager.SingletonInstance.GetCountOfButtonDowns(ControlID, beamID);
        }

        /// <summary>
        /// The number of button presses from a given Beam user since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public uint GetCountOfButtonPresses(uint beamID)
        {
            return BeamManager.SingletonInstance.GetCountOfButtonPresses(ControlID, beamID);
        }

        /// <summary>
        /// The number of button ups from a given Beam user since the last call to DoWork().
        /// </summary>
        /// <param name="beamID">The ID of the Beam user who used the input control.</param>
        public uint GetCountOfButtonUps(uint beamID)
        {
            return BeamManager.SingletonInstance.GetCountOfButtonUps(ControlID, beamID);
        }

        /// <summary>
        /// Trigger a cooldown, disabling this control for a period of time.
        /// </summary>
        /// <param name="cooldown">Duration (in milliseconds) required between triggers.</param>
        public void TriggerCooldown(int cooldown)
        {
            BeamManager.SingletonInstance.TriggerCooldown(ControlID, cooldown);
        }

        internal Int64 cooldownExpirationTime;

        public BeamButtonControl(string controlID, bool disabled, string helpText, string eTag, string sceneID) : base(controlID, disabled, helpText, eTag, sceneID)
        {
        }
    }
}
