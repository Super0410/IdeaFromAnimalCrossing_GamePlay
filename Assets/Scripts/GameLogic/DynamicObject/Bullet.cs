using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperGround;

namespace SuperWeapon
{
    public class Bullet : LogicMatchGraphicMonoPool
    {
        public float speed = 30;
        public float lifeTime = 0.8f;
        public int damage = 1;

        public override void OnReuseObject()
        {
            gameObject.SetActive(true);
            base.CloneGraphic();

            Invoke("Destroy", lifeTime);
        }

        protected override void Destroy()
        {
            gameObject.SetActive(false);
            graphicInstance.SetActive(false);
        }

        private void Update()
        {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
            base.TryResetPos();
        }

        private void OnCollisionEnter(Collision collision)
        {
            IHitable hitable = collision.collider.GetComponent<IHitable>();
            if(hitable != null)
                hitable.TakeDamage(damage);

            Destroy();
        }
    }
}
