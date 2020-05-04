using UnityEngine;

namespace SuperGround
{
    public class GroundManager : Singleton<GroundManager>
    {
        [Header("由于使用的是plane，后续处理时会在tileSize单位的基础上，乘plane的长宽10单位")]
        [Range(1, 10)] public float tileSize = 3;

        public GroundLogic logic = new GroundLogic();
        public GroundGraphic graphic = new GroundGraphic();

        public void Init()
        {
            logic.Init();
            graphic.Init();
        }

        private void Update()
        {
            graphic.SetShaderProperties();
        }

        private void OnValidate()
        {
            logic.OnValueChange();
            graphic.SetShaderProperties();
            graphic.RelocateTile();
        }

        private void OnDrawGizmos()
        {
            graphic.OnDrawGizmos();
        }
    }
}
