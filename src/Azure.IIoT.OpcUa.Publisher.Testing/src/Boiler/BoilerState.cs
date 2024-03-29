/* ========================================================================
 * Copyright (c) 2005-2016 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

namespace Boiler
{
    using Opc.Ua;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public partial class BoilerState
    {
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="node"></param>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);

            Simulation.OnAfterTransition = OnControlSimulation;
            _random = new Random();
        }

        /// <summary>
        /// Cleans up when the object is disposed.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _simulationTimer != null)
            {
                _simulationTimer.Dispose();
                _simulationTimer = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Changes the state of the simulation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="machine"></param>
        /// <param name="transitionId"></param>
        /// <param name="causeId"></param>
        /// <param name="inputArguments"></param>
        /// <param name="outputArguments"></param>
        private ServiceResult OnControlSimulation(
            ISystemContext context,
            StateMachineState machine,
            uint transitionId,
            uint causeId,
            IList<object> inputArguments,
            IList<object> outputArguments)
        {
            switch (causeId)
            {
                case Opc.Ua.Methods.ProgramStateMachineType_Start:
                    {
                        if (_simulationTimer != null)
                        {
                            _simulationTimer.Dispose();
                            _simulationTimer = null;
                        }

                        var updateRate = Simulation.UpdateRate.Value;

                        if (updateRate < 100)
                        {
                            updateRate = 100;
                            Simulation.UpdateRate.Value = updateRate;
                        }

                        _simulationContext = context;
                        _simulationTimer = new Timer(DoSimulation, null, (int)updateRate, (int)updateRate);
                        break;
                    }

                case Opc.Ua.Methods.ProgramStateMachineType_Halt:
                case Opc.Ua.Methods.ProgramStateMachineType_Suspend:
                    {
                        if (_simulationTimer != null)
                        {
                            _simulationTimer.Dispose();
                            _simulationTimer = null;
                        }

                        _simulationContext = context;
                        break;
                    }

                case Opc.Ua.Methods.ProgramStateMachineType_Reset:
                    {
                        if (_simulationTimer != null)
                        {
                            _simulationTimer.Dispose();
                            _simulationTimer = null;
                        }

                        _simulationContext = context;
                        break;
                    }
            }

            return ServiceResult.Good;
        }

        /// <summary>
        /// Rounds a value to the significate digits specified and adds a random perturbation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="significantDigits"></param>
        private double RoundAndPerturb(double value, byte significantDigits)
        {
            double offsetToApply = 0;

            if (!value.Equals(0.0))
            {
                // need to move all significate digits above the decimal point.
                var offset = significantDigits - Math.Log10(Math.Abs(value));

                offsetToApply = Math.Floor(offset);

                if (offsetToApply.Equals(offset))
                {
                    offsetToApply--;
                }
            }

            // round value to significant digits.
            var perturbedValue = Math.Round(value * Math.Pow(10.0, offsetToApply));

            // apply the perturbation.
            perturbedValue += (_random.NextDouble() - 0.5) * 5;

            // restore original exponent.
            return Math.Round(perturbedValue) * Math.Pow(10.0, -offsetToApply);
        }

        /// <summary>
        /// Moves the value towards the target.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <param name="step"></param>
        /// <param name="range"></param>
        private double Adjust(double value, double target, double step, Opc.Ua.Range range)
        {
            // convert percentage step to an absolute step if range is specified.
            if (range != null)
            {
                step *= range.Magnitude;
            }

            var difference = target - value;

            if (difference < 0)
            {
                value -= step;

                if (value < target)
                {
                    return target;
                }
            }
            else
            {
                value += step;

                if (value > target)
                {
                    return target;
                }
            }

            return value;
        }

        /// <summary>
        /// Returns the value as a percentage of the range.
        /// </summary>
        /// <param name="value"></param>
        private double GetPercentage(AnalogItemState<double> value)
        {
            var percentage = value.Value;
            var range = value.EURange.Value;

            if (range != null)
            {
                percentage /= Math.Abs(range.High - range.Low);

                if (Math.Abs(percentage) > 1.0)
                {
                    percentage = 1.0;
                }
            }

            return percentage;
        }

        /// <summary>
        /// Returns the value as a percentage of the range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="range"></param>
        private double GetValue(double value, Opc.Ua.Range range)
        {
            if (range != null)
            {
                return value * range.Magnitude;
            }

            return value;
        }

        /// <summary>
        /// Updates the values for the simulation.
        /// </summary>
        /// <param name="state"></param>
        private void DoSimulation(object state)
        {
            try
            {
                _simulationCounter++;

                // adjust level.
                m_drum.LevelIndicator.Output.Value = Adjust(
                    m_drum.LevelIndicator.Output.Value,
                    m_levelController.SetPoint.Value,
                    0.1,
                    m_drum.LevelIndicator.Output.EURange.Value);

                // calculate inputs for custom controller.
                m_customController.Input1.Value = m_levelController.UpdateMeasurement(m_drum.LevelIndicator.Output);
                m_customController.Input2.Value = GetPercentage(m_inputPipe.FlowTransmitter1.Output);
                m_customController.Input3.Value = GetPercentage(m_outputPipe.FlowTransmitter2.Output);

                // calculate output for custom controller.
                m_customController.ControlOut.Value = (m_customController.Input1.Value +
                    m_customController.Input3.Value - m_customController.Input2.Value) / 2;

                // update flow controller set point.
                m_flowController.SetPoint.Value = GetValue((m_customController.ControlOut.Value + 1) / 2, m_inputPipe.FlowTransmitter1.Output.EURange.Value);

                var error = m_flowController.UpdateMeasurement(m_inputPipe.FlowTransmitter1.Output);

                // adjust the input valve.
                m_inputPipe.Valve.Input.Value = Adjust(m_inputPipe.Valve.Input.Value, (error > 0) ? 100 : 0, 10, null);

                // adjust the input flow.
                m_inputPipe.FlowTransmitter1.Output.Value = Adjust(
                    m_inputPipe.FlowTransmitter1.Output.Value,
                    m_flowController.SetPoint.Value,
                    0.6,
                    m_inputPipe.FlowTransmitter1.Output.EURange.Value);

                // add pertubations.
                m_drum.LevelIndicator.Output.Value = RoundAndPerturb(m_drum.LevelIndicator.Output.Value, 3);
                m_inputPipe.FlowTransmitter1.Output.Value = RoundAndPerturb(m_inputPipe.FlowTransmitter1.Output.Value, 3);
                m_outputPipe.FlowTransmitter2.Output.Value = RoundAndPerturb(m_outputPipe.FlowTransmitter2.Output.Value, 3);

                ClearChangeMasks(_simulationContext, true);
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Unexpected error during boiler simulation.");
            }
        }

        private ISystemContext _simulationContext;
        private Timer _simulationTimer;
        private Random _random;
#pragma warning disable IDE0052 // Remove unread private members
        private long _simulationCounter;
#pragma warning restore IDE0052 // Remove unread private members
    }
}
