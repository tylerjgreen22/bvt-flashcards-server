import Skeleton from "@mui/material/Skeleton";
import Stack from "@mui/material/Stack";

// Loading skeleton for study page
const StudyLoadingSkeleton = () => {
  return (
    <Stack spacing={2} className="flex items-center w-5/6">
      <div>
        <Skeleton
          variant="rounded"
          width={200}
          height={75}
          className="mx-auto"
        />

        <Skeleton
          variant="rounded"
          height={500}
          width={900}
          className="mx-auto mt-4"
        />
      </div>
      <div className="flex justify-between mt-4 w-full">
        <Skeleton variant="rounded" width={75} height={50} />
        <Skeleton variant="rounded" width={75} height={50} />
      </div>

      <Skeleton variant="rounded" height={200} />
      <Skeleton variant="rounded" height={200} />
      <Skeleton variant="rounded" height={200} />
    </Stack>
  );
};

export default StudyLoadingSkeleton;
