using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soco.ShaderVariantsCollection
{
#if UNITY_EDITOR
    public abstract class IExecutable : ScriptableObject
    {
        public abstract void Execute(ShaderVariantCollectionMapper mapper);
    }
#endif
}