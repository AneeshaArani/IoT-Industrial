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

namespace TestData
{
    using Opc.Ua;

    public partial class AnalogArrayValueObjectState
    {
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="node"></param>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);

            InitializeVariable(context, SByteValue, Variables.AnalogArrayValueObjectType_SByteValue);
            InitializeVariable(context, ByteValue, Variables.AnalogArrayValueObjectType_ByteValue);
            InitializeVariable(context, Int16Value, Variables.AnalogArrayValueObjectType_Int16Value);
            InitializeVariable(context, UInt16Value, Variables.AnalogArrayValueObjectType_UInt16Value);
            InitializeVariable(context, Int32Value, Variables.AnalogArrayValueObjectType_Int32Value);
            InitializeVariable(context, UInt32Value, Variables.AnalogArrayValueObjectType_UInt32Value);
            InitializeVariable(context, Int64Value, Variables.AnalogArrayValueObjectType_Int64Value);
            InitializeVariable(context, UInt64Value, Variables.AnalogArrayValueObjectType_UInt64Value);
            InitializeVariable(context, FloatValue, Variables.AnalogArrayValueObjectType_FloatValue);
            InitializeVariable(context, DoubleValue, Variables.AnalogArrayValueObjectType_DoubleValue);
            InitializeVariable(context, NumberValue, Variables.AnalogArrayValueObjectType_NumberValue);
            InitializeVariable(context, IntegerValue, Variables.AnalogArrayValueObjectType_IntegerValue);
            InitializeVariable(context, UIntegerValue, Variables.AnalogArrayValueObjectType_UIntegerValue);
        }

        /// <summary>
        /// Handles the generate values method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="objectId"></param>
        /// <param name="count"></param>
        protected override ServiceResult OnGenerateValues(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            uint count)
        {
            if (context.SystemHandle is not TestDataSystem system)
            {
                return StatusCodes.BadOutOfService;
            }

            GenerateValue(system, SByteValue);
            GenerateValue(system, ByteValue);
            GenerateValue(system, Int16Value);
            GenerateValue(system, UInt16Value);
            GenerateValue(system, Int32Value);
            GenerateValue(system, UInt32Value);
            GenerateValue(system, UInt32Value);
            GenerateValue(system, Int64Value);
            GenerateValue(system, UInt64Value);
            GenerateValue(system, FloatValue);
            GenerateValue(system, DoubleValue);
            GenerateValue(system, NumberValue);
            GenerateValue(system, IntegerValue);
            GenerateValue(system, UIntegerValue);

            return base.OnGenerateValues(context, method, objectId, count);
        }
    }
}
