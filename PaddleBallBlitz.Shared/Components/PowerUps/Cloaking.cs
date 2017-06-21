using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tweens;

namespace PaddleBallBlitz.Components
{
    class Cloaking : Component, IUpdatable
    {
        private float _offset = 0f;
        private Sprite _sprite;
        private float _color = 1f;
        private bool _inTransition = false;
        private float _countdown = .25f;
        public override void onAddedToEntity()
        {
            _sprite = entity.getComponent<Sprite>();
        }

        public void update()
        {
            if (!_inTransition)

                if (_countdown > 0f)
                {
                    _countdown -= Time.deltaTime;
                    return;
                }
                else
                {
                    if (_color >= 1f)
                    {
                        _offset = -.03f;
                        _inTransition = true;
                    }
                    else if (_color <= 0f)
                    {
                        _offset = .03f;
                        _inTransition = true;
                    }
                }

            _color += _offset;

            if (_color <= 0 || _color >= 1f)
            {
                _inTransition = false;
                _countdown = .25f;
            }

            _sprite.color = new Color(_color, _color, _color, _color);
        }
    }
}
