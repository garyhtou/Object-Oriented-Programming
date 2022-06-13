class ampMagnifier
{
private:
	unsigned int count;
	Magnifier *magnifiers;

public:
	ampMagnifier(unsigned int count, unsigned int defaultSize, double defaultScale)
	{
		magnifiers = Magnifiers[count];
		for (int i = 0; i < count; i++)
		{
			magnifiers[i] = Magnifier(defaultSize, defaultScale);
		}
	}

	double scaledSize()
	{
		double composite = 0;
		bool foundActive = false;

		for (int i = 0; i < count; i++)
		{
			Magnifier m = magnifiers[i];
			if (m.inactive())
			{
				continue;
			}
			foundActive = true;

			composite += m.scaledSize();
		}

		if (!foundActive)
		{
			throw "No active Magnifiers";
		}
		return composite;
	}
}
