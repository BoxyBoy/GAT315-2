using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEffector : Force
{
	public CircleShape shape;

	public Vector2 position { get => shape.transform.position; set => shape.transform.position = value; }
	public float forceMagnitude { get; set; }
	public ForceModeData.eType forceMode { get; set; }

	public override void ApplyForce(Body body)
	{
		Circle circleA = new Circle(position, shape.radius);
		Circle circleB = new Circle(body.position, body.shape.size / 2);
		if (circleA.Contains(circleB))
		{
			Vector2 direction = body.position - position;//< direction vector body position(head) from position (tail) >;
			float distance = direction.magnitude;//< distance of direction vector >;
			float t = distance / shape.size;//< normalized distance(0 - 1) by dividing distance by shape.radius >;
			Vector2 force = direction.normalized;//< normalized direction >;

			switch (forceMode)
			{
				case ForceModeData.eType.Constant:
					force *= forceMagnitude;
					break;
				case ForceModeData.eType.InverseLinear:
					force *= ((1 - t) * forceMagnitude);
					break;
				case ForceModeData.eType.InverseSquared:
					force *= (((1 - t) * (1 - t)) * forceMagnitude);
					break;
			}

			body.AddForce(force);
		}
	}
}
