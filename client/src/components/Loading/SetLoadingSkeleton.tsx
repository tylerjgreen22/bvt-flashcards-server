import Skeleton from "@mui/material/Skeleton";
import Stack from "@mui/material/Stack";

// Loading skeleton for sets
const SetLoadingSkeleton = () => {
  return (
    <Stack spacing={2}>
      <Skeleton variant="rounded" width={90} height={30} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <Skeleton variant="rounded" height={70} />
      <div>
        <Skeleton
          variant="rounded"
          width={150}
          height={50}
          className="mx-auto"
        />
      </div>
    </Stack>
  );
};

export default SetLoadingSkeleton;
