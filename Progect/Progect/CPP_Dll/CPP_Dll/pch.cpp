#include "pch.h"

extern "C" _declspec(dllexport) int Intagrate(const float* Array, const int nx, const int ny, const double stepX, const int count, const float* left, const float* right, float* res)
{
	DFTaskPtr task;
	float border[2] = { 0, (nx - 1) * (float)stepX };
	int status = dfsNewTask1D(&task, nx, border, DF_UNIFORM_PARTITION, ny * 2, Array, DF_NO_HINT);
	if (status != DF_STATUS_OK)
		return 1;
	float* scoeff = new float[ny * 2 * DF_PP_CUBIC * (nx - 1)];
	status = dfsEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END, NULL, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
	if (status != DF_STATUS_OK)
		return 2;
	status = dfsConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
	if (status != DF_STATUS_OK)
		return 3;
	status = dfsIntegrate1D(task, DF_METHOD_PP, count, left, DF_NO_HINT, right, DF_NO_HINT, NULL, NULL, res, DF_NO_HINT);
	if (status != DF_STATUS_OK)
		return 4;
	status = dfDeleteTask(&task);
	return status;
}